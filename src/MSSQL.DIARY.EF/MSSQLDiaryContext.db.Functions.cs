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
        public List<FunctionDependencies> GetFunctionDependencies(string astrFunctionName, string functionType)
        {
            if (astrFunctionName == null) throw new ArgumentNullException(nameof(astrFunctionName));
            var lstInterdependency = new List<FunctionDependencies>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    try
                    {
                        var commad = conn.CreateCommand();
                        var newFunctionName = astrFunctionName.Replace(
                            astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) +
                            ".", "");
                        commad.CommandText = SqlQueryConstant.GetFunctionDependencies
                            .Replace("@function_Type", "'" + functionType + "'")
                            .Replace("@function_name", "'" + newFunctionName + "'");
                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();

                        using (var reader = commad.ExecuteReader())
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

        public List<FunctionProperties> GetFunctionProperties(string astrFunctionName, string functionType)
        {
            var lstFunctionProperties = new List<FunctionProperties>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    var newFunctionName = astrFunctionName.Replace(
                        astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) + ".",
                        "");
                    commad.CommandText = SqlQueryConstant.GetFunctionProperties
                        .Replace("@function_Type", "'" + functionType + "'")
                        .Replace("@function_name", "'" + newFunctionName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();


                    using (var reader = commad.ExecuteReader())
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

        public List<FunctionParameters> GetFunctionParameters(string astrFunctionName, string functionType)
        {
            var lstFunctionColumns = new List<FunctionParameters>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetFunctionParameters
                        .Replace("@function_Type", "'" + functionType + "'")
                        .Replace("@function_name", "'" + astrFunctionName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
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

        public FunctionCreateScript GetFunctionCreateScript(string astrFunctionName, string functionType)
        {
            var createScript = new FunctionCreateScript();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetFunctionCreateScript
                        .Replace("@function_Type", "'" + functionType + "'")
                        .Replace("@function_name", "'" + astrFunctionName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                createScript.createFunctionscript = reader.SafeGetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return createScript;
        }

        public List<PropertyInfo> GetAllFunctionWithMsDescriptions(string functionType)
        {
            var getAllTbleDesc = new List<PropertyInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetAllFunctionWithMsDescriptions.Replace("@function_Type",
                            "'" + functionType + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getAllTbleDesc.Add(new PropertyInfo
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

            return getAllTbleDesc;
        }

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

        private void CreateFunctionDescription(string astrDescriptionValue, string astrSchemaName,string astrFunctioneName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrFunctioneName.Replace(
                        astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".",
                        "");

                commad.CommandText = SqlQueryConstant
                    .UpdateFunctionExtendedProperty
                    .Replace("@fun_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@FunctionName", "'" + tableName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void UpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName, string astrFunctioneName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrFunctioneName.Replace(
                        astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".",
                        "");

                commad.CommandText = SqlQueryConstant
                    .UpdateFunctionExtendedProperty
                    .Replace("@fun_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@FunctionName", "'" + tableName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }
    }
}