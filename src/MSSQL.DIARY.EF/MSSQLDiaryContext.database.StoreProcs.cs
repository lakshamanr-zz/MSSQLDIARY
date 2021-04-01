using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetStoreProcedures()
        {
            var storeProcedures = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetStoreProcedures;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                storeProcedures.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return storeProcedures;
        }


        public List<PropertyInfo> GetAllStoreprocedureDescription()
        {
            var getAllTableDesc = new List<PropertyInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAllStoreProcWithMsDesc;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getAllTableDesc.Add(new PropertyInfo
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

            return getAllTableDesc;
        }

        public Ms_Description GetCreateScriptOfStoreProc(string StoreprocName)
        {
            var getStoreProcInfos = new List<PropertyInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetCreateScriptOfStoreProc.Replace("@StoreprocName",
                            "'" + StoreprocName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getStoreProcInfos.Add(new PropertyInfo
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

            return new Ms_Description {desciption = getStoreProcInfos.FirstOrDefault()?.istrValue};
        }

        public List<SP_Depencancy> GetStoreProcDependancy(string storeprocName)
        {
            var getSpDependancies = new List<SP_Depencancy>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetStoreProcDependencies.Replace("@StoreprocName", "'" + storeprocName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getSpDependancies.Add(new SP_Depencancy
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

            return getSpDependancies;
        }

        public List<SP_Parameters> GetStoreProcParameters(string storeprocName)
        {
            var getSpParameters = new List<SP_Parameters>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetAllStoreProcParamWithMsDesc.Replace("@StoreprocName",
                            "'" + storeprocName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getSpParameters.Add(new SP_Parameters
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

            return getSpParameters;
        }

        public List<ExecutionPlanInfo> GetCachedExecutionPlan(string storeprocName)
        {
            var exeutionPlan = new List<ExecutionPlanInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    //tables.Replace(tables.Substring(0, tables.IndexOf("."))+".", ""
                    var newStoreprocName =
                        storeprocName.Replace(storeprocName.Substring(0, storeprocName.IndexOf(".")) + ".", "");
                    commad.CommandText =
                        SqlQueryConstant.GetExecutionPlanOfStoreProc.Replace("@StoreprocName",
                            "'" + newStoreprocName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                exeutionPlan.Add(new ExecutionPlanInfo
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

            return exeutionPlan;
        }

        public void CreateOrUpdateStoreProcDescription(string astrDescriptionValue, string astrSchemaName,string storeprocName, string parameterName = null)
        {
            try
            {
                UpdateStoreProcDescription(astrDescriptionValue, astrSchemaName, storeprocName, parameterName);
            }
            catch (Exception)
            {
                CreateStoreprocDescription(astrDescriptionValue, astrSchemaName, storeprocName, parameterName);
            }
        }

        public string GetStoreProcMsDescription(string StoreprocName)
        {
            var strSpDescription = "";
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetStoreProcMsDescription.Replace("@StoreprocName", "'" + StoreprocName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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

        private void UpdateStoreProcDescription(string astrDescriptionValue, string astrSchemaName,string storeprocName, string parameterName = null)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var spName =
                    storeprocName.Replace(
                        storeprocName.Substring(0, storeprocName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                if (parameterName == null)
                    commad.CommandText = SqlQueryConstant
                        .UpdateStoreProcExtendedProperty
                        .Replace("@sp_value", "'" + astrDescriptionValue + "'")
                        .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                        .Replace("@sp_Name", "'" + spName + "'");
                else
                    commad.CommandText = SqlQueryConstant
                            .UpdateStoreProcParameterExtendedProperty
                            .Replace("@sp_value", "'" + astrDescriptionValue + "'")
                            .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                            .Replace("@sp_Name", "'" + spName + "'")
                            .Replace("@parmeterName", "'" + parameterName + "'")
                        ;


                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void CreateStoreprocDescription(string astrDescriptionValue, string astrSchemaName,string storeprocName, string parameterName = null)
        {
            var spName =
                storeprocName.Replace(
                    storeprocName.Substring(0, storeprocName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                if (parameterName == null)
                    commad.CommandText = SqlQueryConstant
                        .InsertStoreProcExtendedProperty
                        .Replace("@sp_value", "'" + astrDescriptionValue + "'")
                        .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                        .Replace("@sp_Name", "'" + spName + "'");
                else
                    commad.CommandText = SqlQueryConstant
                            .InsertStoreProcParameterExtendedProperty
                            .Replace("@sp_value", "'" + astrDescriptionValue + "'")
                            .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                            .Replace("@sp_Name", "'" + spName + "'")
                            .Replace("@parmeterName", "'" + parameterName + "'")
                        ;


                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                try
                {
                    commad.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}