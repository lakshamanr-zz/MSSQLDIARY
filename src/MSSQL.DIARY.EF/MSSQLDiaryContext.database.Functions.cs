using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {
        /// <summary>
        /// In this method we are Find the function and there dependencies.
        /// </summary>
        /// <param name="astrFunctionName"></param>
        /// <param name="astrFunctionType"></param>
        /// <returns></returns>
        public List<FunctionDependencies> GetFunctionDependencies(string astrFunctionName, string astrFunctionType)
        { 
            var lstInterdependency = new List<FunctionDependencies>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    try
                    {
                        var command = lDbConnection.CreateCommand();
                        var newFunctionName = astrFunctionName.Replace(astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) +".", "");
                        command.CommandText = SqlQueryConstant.GetFunctionDependencies.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + newFunctionName + "'");
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstInterdependency.Add(new FunctionDependencies
                                    {
                                        name = reader.SafeGetString(0)
                                    });
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstInterdependency.Distinct().ToList();
        }

        /// <summary>
        /// In this method we are get the function properties.
        /// </summary>
        /// <param name="astrFunctionName"></param>
        /// <param name="astrFunctionType"></param>
        /// <returns></returns>
        public List<FunctionProperties> GetFunctionProperties(string astrFunctionName, string astrFunctionType)
        {
            var lstFunctionProperties = new List<FunctionProperties>();
            try
            {
                using (var  lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    var newFunctionName = astrFunctionName.Replace(astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                    command.CommandText = SqlQueryConstant.GetFunctionProperties.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + newFunctionName + "'");
                    Database.OpenConnection();  
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstFunctionProperties.Add(new FunctionProperties
                                {
                                    uses_ansi_nulls = reader.SafeGetString(0),
                                    uses_quoted_identifier = reader.SafeGetString(1),
                                    create_date = reader.SafeGetString(2),
                                    modify_date = reader.SafeGetString(3)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstFunctionProperties;
        }
        /// <summary>
        /// In this method we getting the function parameters list.
        /// </summary>
        /// <param name="astrFunctionName"></param>
        /// <param name="astrFunctionType"></param>
        /// <returns></returns>
        public List<FunctionParameters> GetFunctionParameters(string astrFunctionName, string astrFunctionType)
        {
            var lstFunctionColumns = new List<FunctionParameters>();
            try
            {
                using (var  lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetFunctionParameters.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + astrFunctionName + "'"); 
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstFunctionColumns.Add(new FunctionParameters
                                {
                                    name = reader.SafeGetString(0),
                                    type = reader.SafeGetString(1),
                                    updated = reader.SafeGetString(2),
                                    selected = reader.SafeGetString(3),
                                    column_name = reader.SafeGetString(7)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            lstFunctionColumns.ForEach(x => { x.name = x.name == string.Empty ? "@Return Parameter " : x.name; });
            return lstFunctionColumns;
        }

        /// <summary>
        /// In this method returns the create script of the function
        /// </summary>
        /// <param name="astrFunctionName"></param>
        /// <param name="astrFunctionType"></param>
        /// <returns></returns>
        public FunctionCreateScript GetFunctionCreateScript(string astrFunctionName, string astrFunctionType)
        {
            var lstrFunctionCreateScript = new FunctionCreateScript();
            try
            {
                using (var  lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetFunctionCreateScript.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + astrFunctionName + "'"); 
                    Database.OpenConnection(); 
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                            {
                                lstrFunctionCreateScript.createFunctionscript = reader.SafeGetString(0);
                            } 
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstrFunctionCreateScript;
        }

        /// <summary>
        /// This method return the functions with it description
        /// </summary>
        /// <param name="astrFunctionType"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetFunctionsWithDescription(string astrFunctionType)
        {
            var lstFunctionDescriptions = new List<PropertyInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetFunctionsWithDescription.Replace("@function_Type", "'" + astrFunctionType + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstFunctionDescriptions.Add(new PropertyInfo
                                {
                                    istrName = reader.SafeGetString(0),
                                    istrValue = reader.GetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstFunctionDescriptions;
        }

        /// <summary>
        /// Update or Create new description for the function
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrFunctioneName"></param>
        public void CreateOrUpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName,string astrFunctioneName)
        {
            try
            {
                UpdateFunctionDescription(astrDescriptionValue, astrSchemaName, astrFunctioneName);
            }
            catch (Exception)
            {
                CreateFunctionDescription(astrDescriptionValue, astrSchemaName, astrFunctioneName);
            }
        }

        /// <summary>
        /// Create description for the function
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrFunctioneName"></param>
        private void CreateFunctionDescription(string astrDescriptionValue, string astrSchemaName,string astrFunctioneName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var tableName = astrFunctioneName.Replace(astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.UpdateFunctionExtendedProperty.Replace("@fun_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@FunctionName", "'" + tableName + "'");
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update the function descriptions.
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrFunctioneName"></param>
        private void UpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName, string astrFunctioneName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var tableName = astrFunctioneName.Replace(astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.UpdateFunctionExtendedProperty.Replace("@fun_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@FunctionName", "'" + tableName + "'");
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get scalar functions
        /// </summary>
        /// <returns></returns>
        public List<string> GetScalarFunctions()
        {
            var lstScalarFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetScalarFunctions;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstScalarFunctions.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return lstScalarFunctions;
        }

        /// <summary>
        /// Get Table value functions
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableValueFunctions()
        {
            var lstTableValueFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableValueFunctions;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableValueFunctions.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return lstTableValueFunctions;
        }

        /// <summary>
        /// Get aggregation functions
        /// </summary>
        /// <returns></returns>
        public List<string> GetAggregateFunctions()
        {
            var lstAggregateFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAggregateFunctions;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstAggregateFunctions.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstAggregateFunctions;
        }

        /// <summary>
        ///  Get user defined data type.
        /// </summary>
        /// <returns></returns>

    }
}