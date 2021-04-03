using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {

        /// <summary>
        /// Get store procedures.
        /// </summary>
        /// <returns></returns>
        public List<string> GetStoreProcedures()
        {
            var lstStoreProcedures = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProcedures; 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstStoreProcedures.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstStoreProcedures;
        }

        /// <summary>
        /// Get store procedures with descriptions
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetStoreProceduresWithDescription()
        {
            var lstStoreProceduresWithDescription = new List<PropertyInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProceduresWithDescription; 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstStoreProceduresWithDescription.Add(new PropertyInfo
                                {
                                    istrName = reader.SafeGetString(0),
                                    istrValue = reader.SafeGetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstStoreProceduresWithDescription;
        }
        /// <summary>
        /// Get create script of the store procedure
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public Ms_Description GetStoreProcedureCreateScript(string astrStoreProcedureName)
        {
            var lstrStoreProcedureCreateScript = new List<PropertyInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProcedureCreateScript.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'"); 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstrStoreProcedureCreateScript.Add(new PropertyInfo
                                {
                                    istrName = reader.SafeGetString(0),
                                    istrValue = reader.SafeGetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new Ms_Description {desciption = lstrStoreProcedureCreateScript.FirstOrDefault()?.istrValue};
        }

        /// <summary>
        /// Get store procedure dependencies
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<SP_Depencancy> GetStoreProceduresDependency(string astrStoreProcedureName)
        {
            var lstStoreProcedureDependencies = new List<SP_Depencancy>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProcDependencies.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstStoreProcedureDependencies.Add(new SP_Depencancy
                                {
                                    referencing_object_name = reader.SafeGetString(0),
                                    referencing_object_type = reader.SafeGetString(1),
                                    referenced_object_name = reader.SafeGetString(2)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstStoreProcedureDependencies;
        }
        /// <summary>
        /// Get store procedures parameters with Descriptions
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<SP_Parameters> GetStoreProceduresParametersWithDescription(string astrStoreProcedureName)
        {
            var lstStoreProceduresParametersWithDescriptions = new List<SP_Parameters>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProceduresParametersWithDescriptions.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'"); 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstStoreProceduresParametersWithDescriptions.Add(new SP_Parameters
                                {
                                    Parameter_name = reader.SafeGetString(0),
                                    Type = reader.SafeGetString(1),
                                    Length = reader.SafeGetString(2),
                                    Prec = reader.SafeGetString(3),
                                    Scale = reader.SafeGetString(4),
                                    Param_order = reader.SafeGetString(5),
                                    Extended_property = reader.SafeGetString(7)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstStoreProceduresParametersWithDescriptions;
        }

        /// <summary>
        /// Get store procedure execution plan details
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<ExecutionPlanInfo> GetStoreProcedureExecutionPlan(string astrStoreProcedureName)
        {
            var lstExecutionPlanDetails = new List<ExecutionPlanInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                { 
                    var lStoreProcedureName = astrStoreProcedureName.Replace(astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                    command.CommandText = SqlQueryConstant.GetExecutionPlanOfStoreProc.Replace("@StoreprocName", "'" + lStoreProcedureName + "'"); 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstExecutionPlanDetails.Add(new ExecutionPlanInfo
                                {
                                    QueryPlanXML = reader.SafeGetString(0),
                                    UseAccounts = reader.SafeGetString(1),
                                    CacheObjectType = reader.SafeGetString(2),
                                    Size_In_Byte = reader.SafeGetString(3),
                                    SqlText = reader.SafeGetString(4)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstExecutionPlanDetails;
        }

        /// <summary>
        /// Create or Update store procedure description
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <param name="astrParameterName"></param>
        public void CreateOrUpdateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName,string astrStoreProcedureName, string astrParameterName = null)
        {
            try
            {
                UpdateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName, astrParameterName);
            }
            catch (Exception)
            {
                CreateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName, astrParameterName);
            }
        }

        /// <summary>
        /// Update the store procedure descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <param name="astrParameterName"></param>
        private void UpdateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName,string astrStoreProcedureName, string astrParameterName = null)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var lStoreProcedureName = astrStoreProcedureName.Replace(astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = astrParameterName == null ? SqlQueryConstant.UpdateStoreProcExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'") : SqlQueryConstant.UpdateStoreProcParameterExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'").Replace("@parmeterName", "'" + astrParameterName + "'"); 
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a store  procedure descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <param name="astrParameterName"></param>
        private void CreateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName,string astrStoreProcedureName, string astrParameterName = null)
        {
            var lStoreProcedureName = astrStoreProcedureName.Replace(    astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = astrParameterName == null ? SqlQueryConstant.InsertStoreProcExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'") : SqlQueryConstant.InsertStoreProcParameterExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'").Replace("@parmeterName", "'" + astrParameterName + "'");  
                Database.OpenConnection();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Get store procedure descriptions
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public string GetStoreProcedureDescription(string astrStoreProcedureName)
        {
            var strSpDescription = "";
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProcMsDescription.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                strSpDescription = reader.SafeGetString(1);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return strSpDescription;
        }
    }
}