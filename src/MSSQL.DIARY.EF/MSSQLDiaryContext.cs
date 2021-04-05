using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DataColumn = System.Data.DataColumn;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext : DbContext
    {
        public MsSqlDiaryContext(string astrDatabaseConnection = null)
        {
            IstrDatabaseConnection = astrDatabaseConnection;
        }

        public MsSqlDiaryContext(DbContextOptions<MsSqlDiaryContext> options) : base(options)
        {
        }
        public string IstrDatabaseConnection { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                    if (IstrDatabaseConnection.IsNullOrWhiteSpace())
                    {
                        //  optionsBuilder.UseSqlServer($"Server=DESKTOP-NFUD15G\\SQLEXPRESS;Database=AdventureWorks2016;User Id=mssql; Password=mssql;Trusted_Connection=false;");
                    }
                    else
                    {
                        optionsBuilder.UseSqlServer(IstrDatabaseConnection);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        /// <summary>
        /// Get the database Properties
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseProperties()
        {
            var lstDatabaseProperties = new List<PropertyInfo>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetDatabaseProperties.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        for (var i = 0; i < 12; i++)
                            lstDatabaseProperties.Add(new PropertyInfo
                            {
                                istrName = reader.GetName(i),
                                istrValue = reader.GetString(i)
                            });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstDatabaseProperties;
        }

        /// <summary>
        /// Get the database options
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseOptions()
        {
            var lstDatabaseOptions = new List<PropertyInfo>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetDatabaseOptions.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        for (var i = 0; i < 14; i++)
                            lstDatabaseOptions.Add(new PropertyInfo
                            {
                                istrName = reader.GetName(i),
                                istrValue = reader.GetString(i)
                            });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstDatabaseOptions;
        }
        /// <summary>
        /// Get list of database files
        /// </summary>
        /// <returns></returns>

        public List<FileInfomration> GetDatabaseFiles()
        {
            var lstDatabaseFiles = new List<FileInfomration>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetDatabaseFiles.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                Database.OpenConnection();
                DataTable ldtDatabaseFiles = new DataTable();
                ldtDatabaseFiles.Load(command.ExecuteReader());
                Type lTypeFileInformation = typeof(FileInfomration);
                FileInfomration lFileInfomration = new FileInfomration();
                if (ldtDatabaseFiles.IsNotNull()&& ldtDatabaseFiles.Rows.Count>0)
                {
                    foreach (DataRow ldtRow in ldtDatabaseFiles.Rows)
                    {
                        foreach (DataColumn ldtColumn in ldtDatabaseFiles.Columns)
                        {
                            if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                            {
                                var piShared = lTypeFileInformation.GetProperty(ldtColumn.ColumnName);
                                piShared.SetValue(lFileInfomration, ldtRow[ldtColumn]);
                            } 
                        }
                        lstDatabaseFiles.Add(lFileInfomration);
                    }
                    
                } 
            }
            catch (Exception)
            {
                // ignored
            }

            return lstDatabaseFiles;
        }

        /// <summary>
        /// Get database names.
        /// </summary>
        /// <returns></returns>
        public List<DatabaseName> GetDatabaseNames()
        {
            var lstDatabaseNames = new List<DatabaseName>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetDatabaseNames;
                Database.OpenConnection();
                DataTable ldtDatabaseNames = new DataTable();
                ldtDatabaseNames.Load(command.ExecuteReader());
                Type lTypeDatabaseName = typeof(DatabaseName);
                DatabaseName lDatabaseName = new DatabaseName();
                if (ldtDatabaseNames.IsNotNull() && ldtDatabaseNames.Rows.Count > 0)
                {
                    foreach (DataRow ldtRow in ldtDatabaseNames.Rows)
                    {
                        lDatabaseName = new DatabaseName();
                        foreach (DataColumn ldtColumn in ldtDatabaseNames.Columns)
                        {
                            if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                            {
                                var piShared = lTypeDatabaseName.GetProperty(ldtColumn.ColumnName);
                                piShared.SetValue(lDatabaseName, ldtRow[ldtColumn]);
                            }
                        }
                        lstDatabaseNames.Add(lDatabaseName);
                    }

                }
                
            }
            catch (Exception ex)
            {
                // ignored
            }

            return lstDatabaseNames;
        }
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
                using var lDbConnection = Database.GetDbConnection();
                try
                {
                    var command = lDbConnection.CreateCommand();
                    var newFunctionName = astrFunctionName.Replace(astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                    command.CommandText = SqlQueryConstant.GetFunctionDependencies.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + newFunctionName + "'");
                    Database.OpenConnection();
                    DataTable ldtFunctionDependencies = new DataTable();
                    ldtFunctionDependencies.Load(command.ExecuteReader());
                    FunctionDependencies lFunctionDependencies = new FunctionDependencies();
                    if (ldtFunctionDependencies.IsNotNull() && ldtFunctionDependencies.Rows.Count > 0)
                    {
                        foreach (DataRow ldtRow in ldtFunctionDependencies.Rows)
                        {
                            foreach (DataColumn ldtColumn in ldtFunctionDependencies.Columns)
                            {
                                if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                                {
                                    var piShared = ldtFunctionDependencies.GetProperty(ldtColumn.ColumnName);
                                    piShared.SetValue(lFunctionDependencies, ldtRow[ldtColumn]);
                                }
                            }
                            lstInterdependency.Add(lFunctionDependencies);
                        }

                    }
                    
                }
                catch (Exception)
                {
                    // ignored
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
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                var newFunctionName = astrFunctionName.Replace(astrFunctionName.Substring(0, astrFunctionName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.GetFunctionProperties.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + newFunctionName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
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
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetFunctionParameters.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + astrFunctionName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
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
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetFunctionCreateScript.Replace("@function_Type", "'" + astrFunctionType + "'").Replace("@function_name", "'" + astrFunctionName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        lstrFunctionCreateScript.createFunctionscript = reader.SafeGetString(0);
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
                using var command = Database.GetDbConnection().CreateCommand();
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
        public void CreateOrUpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName, string astrFunctioneName)
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
        private void CreateFunctionDescription(string astrDescriptionValue, string astrSchemaName, string astrFunctioneName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var tableName = astrFunctioneName.Replace(astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = SqlQueryConstant.UpdateFunctionExtendedProperty.Replace("@fun_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@FunctionName", "'" + tableName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update the function descriptions.
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrFunctioneName"></param>
        private void UpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName, string astrFunctioneName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var tableName = astrFunctioneName.Replace(astrFunctioneName.Substring(0, astrFunctioneName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = SqlQueryConstant.UpdateFunctionExtendedProperty.Replace("@fun_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@FunctionName", "'" + tableName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetScalarFunctions;
                Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            lstScalarFunctions.Add(reader.GetString(0));
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableValueFunctions;
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstTableValueFunctions.Add(reader.GetString(0));
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetAggregateFunctions;
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstAggregateFunctions.Add(reader.GetString(0));
            }
            catch (Exception)
            {
                // ignored
            }
            return lstAggregateFunctions;
        }

        /// <summary>
        /// Get list of schemas and there description
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetSchemaWithDescriptions()
        {
            var lstPropInfo = new List<PropertyInfo>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetSchemaWithDescriptions;
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstPropInfo.Add(new PropertyInfo
                            {
                                istrName = reader.GetString(0),
                                istrValue = reader.GetString(1)
                            }
                        );
            }
            catch (Exception)
            {
                // ignored
            }

            return lstPropInfo;
        }

        /// <summary>
        /// Create or update the schema descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        public void CreateOrUpdateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            try
            {
                UpdateSchemaDescription(astrDescriptionValue, astrSchemaName);
            }
            catch (Exception)
            {
                CreateSchemaDescription(astrDescriptionValue, astrSchemaName);
            }
        }

        /// <summary>
        /// Update schema Descriptions 
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void CreateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = SqlQueryConstant.CreateSchemaColumnExtendedProperty.Replace("@Schema_info", "'" + astrDescriptionValue + "'").Replace("@SchemaName", "'" + astrSchemaName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update the schema description
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void UpdateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = SqlQueryConstant.UpdateSchemaColumnExtendedProperty.Replace("@Schema_info", "'" + astrDescriptionValue + "'").Replace("@SchemaName", "'" + astrSchemaName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get schema references with table / view / store procedures etc. 
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<SchemaReferanceInfo> GetSchemaReferences(string astrSchemaName)
        {
            var lstSchemaReferences = new List<SchemaReferanceInfo>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetSchemaReferences.Replace("@schema_id", "'" + astrSchemaName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstSchemaReferences.Add(new SchemaReferanceInfo
                            {
                                istrName = reader.GetString(0)
                            }
                        );
            }
            catch (Exception)
            {
                // ignored
            }

            return lstSchemaReferences;
        }

        /// <summary>
        /// Get the schema description.
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public Ms_Description GetSchemaDescription(string astrSchemaName)
        {
            var schemaDescription = new Ms_Description();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetSchemaMsDescription.Replace("@schemaName", "'" + astrSchemaName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        schemaDescription.desciption = reader.GetString(0);
            }
            catch (Exception)
            {
                // ignored
            }

            return schemaDescription;
        }

        //public SchemaCreateScript GetSchemaCreateSript()
        //{
        //    var sch_cs = new SchemaCreateScript();
        //    try
        //    {
        //        using (var command = Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = SqlQueryConstant.GetServerName;
        //            Database.OpenConnection();
        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                    while (reader.Read())
        //                    {

        //                        sch_cs.istrCreateScript = reader.GetString(0);
        //                    }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return sch_cs;
        //}

        /// <summary>
        /// Get server name.
        /// </summary>
        /// <returns></returns>
        public string GetServerName()
        {
            var lstrServerName = "";
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetServerName;
                Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            lstrServerName = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }

            return lstrServerName;
        }

        /// <summary>
        /// Get xml schemas
        /// </summary>
        /// <returns></returns>

        public List<string> GetXmlSchemas()
        {
            var lstXmlSchemas = new List<string>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetXmlSchemas;
                Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        while (reader.Read())
                            lstXmlSchemas.Add(reader.GetString(0));
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return lstXmlSchemas;
        }

        /// <summary>
        /// Get server Properties
        /// </summary>
        /// <returns></returns>

        public List<PropertyInfo> GetServerProperties()
        {
            var lstServerProperties = new List<PropertyInfo>();
            try
            {
                for (int count = 0; count < SqlQueryConstant.GetServerProperties.Count(); count++)
                {
                    using var command = Database.GetDbConnection().CreateCommand();
                    command.CommandText = SqlQueryConstant.GetServerProperties[count];
                    Database.OpenConnection();
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows)
                        while (reader.Read())
                            lstServerProperties.Add(new PropertyInfo
                            {
                                istrName = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).FirstOrDefault(),
                                istrValue = reader.GetString(0).Replace("\0", "")
                            });
                }

            }
            catch (Exception)
            {
                // ignored
            }

            return lstServerProperties;
        }

        /// <summary>
        /// Get server advance properties.
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            var lstAdvancedServerSettings = new List<PropertyInfo>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetAdvancedServerSettings;
                Database.OpenConnection();
                DataTable ldtAdvServerSetting = new DataTable();
                ldtAdvServerSetting.Load(command.ExecuteReader());
                Type lTypePropertyInfo = typeof(PropertyInfo);
                PropertyInfo lPropertyInfo = new PropertyInfo();
                if (ldtAdvServerSetting.IsNotNull() && ldtAdvServerSetting.Rows.Count > 0)
                {
                    foreach (DataRow ldtRow in ldtAdvServerSetting.Rows)
                    {
                        lPropertyInfo = new PropertyInfo();
                        foreach (DataColumn ldtColumn in ldtAdvServerSetting.Columns)
                        {
                            if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                            {
                                var piShared = lTypePropertyInfo.GetProperty(ldtColumn.ColumnName);
                                piShared.SetValue(lPropertyInfo, ldtRow[ldtColumn]);
                            }
                        }
                        lstAdvancedServerSettings.Add(lPropertyInfo);
                    }

                } 
            }
            catch (Exception)
            {
                // ignored
            }
            return lstAdvancedServerSettings;
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetStoreProceduresWithDescription;
                Database.OpenConnection(); 
                DataTable ldtStoreProcedures = new DataTable();
                ldtStoreProcedures.Load(command.ExecuteReader());
                Type lTypePropertyInfo = typeof(PropertyInfo);
                PropertyInfo lPropertyInfo;
                if (ldtStoreProcedures.IsNotNull() && ldtStoreProcedures.Rows.Count > 0)
                {
                    foreach (DataRow ldtRow in ldtStoreProcedures.Rows)
                    {
                        lPropertyInfo = new PropertyInfo();
                        foreach (DataColumn ldtColumn in ldtStoreProcedures.Columns)
                        {
                            if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                            {
                                var piShared = lTypePropertyInfo.GetProperty(ldtColumn.ColumnName);
                                piShared.SetValue(lPropertyInfo, ldtRow[ldtColumn]);
                            }
                        }
                        lstStoreProceduresWithDescription.Add(lPropertyInfo);
                    } 
                } 
            }
            catch (Exception ex)
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetStoreProcedureCreateScript.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                Database.OpenConnection();
                DataTable ldtStoreProcedureCreateScript = new DataTable();
                ldtStoreProcedureCreateScript.Load(command.ExecuteReader()); 
                Type lTypePropertyInfo = typeof(PropertyInfo);
                PropertyInfo lPropertyInfo;
                if (ldtStoreProcedureCreateScript.IsNotNull() && ldtStoreProcedureCreateScript.Rows.Count > 0)
                {
                    foreach (DataRow ldtRow in ldtStoreProcedureCreateScript.Rows)
                    {
                        lPropertyInfo = new PropertyInfo();
                        foreach (DataColumn ldtColumn in ldtStoreProcedureCreateScript.Columns)
                        {
                            if (!Convert.IsDBNull(ldtRow[ldtColumn]))
                            {
                                var piShared = lTypePropertyInfo.GetProperty(ldtColumn.ColumnName);
                                piShared.SetValue(lPropertyInfo, ldtRow[ldtColumn]);
                            }
                        }
                        lstrStoreProcedureCreateScript.Add(lPropertyInfo);
                    }
                }

            }
            catch (Exception)
            {
                // ignored
            }

            return new Ms_Description { desciption = lstrStoreProcedureCreateScript.FirstOrDefault()?.istrValue };
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetStoreProcDependencies.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstStoreProcedureDependencies.Add(new SP_Depencancy
                        {
                            referencing_object_name = reader.SafeGetString(0),
                            referencing_object_type = reader.SafeGetString(1),
                            referenced_object_name = reader.SafeGetString(2)
                        });
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetStoreProceduresParametersWithDescriptions.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
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
                using var command = Database.GetDbConnection().CreateCommand();
                var lStoreProcedureName = astrStoreProcedureName.Replace(astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.GetExecutionPlanOfStoreProc.Replace("@StoreprocName", "'" + lStoreProcedureName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
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
        public void CreateOrUpdateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName, string astrStoreProcedureName, string astrParameterName = null)
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
        private void UpdateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName, string astrStoreProcedureName, string astrParameterName = null)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var lStoreProcedureName = astrStoreProcedureName.Replace(astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = astrParameterName == null ? SqlQueryConstant.UpdateStoreProcExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'") : SqlQueryConstant.UpdateStoreProcParameterExtendedProperty.Replace("@sp_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@sp_Name", "'" + lStoreProcedureName + "'").Replace("@parmeterName", "'" + astrParameterName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a store  procedure descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <param name="astrParameterName"></param>
        private void CreateStoreProcedureDescription(string astrDescriptionValue, string astrSchemaName, string astrStoreProcedureName, string astrParameterName = null)
        {
            var lStoreProcedureName = astrStoreProcedureName.Replace(astrStoreProcedureName.Substring(0, astrStoreProcedureName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using var command = Database.GetDbConnection().CreateCommand();
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
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetStoreProcMsDescription.Replace("@StoreprocName", "'" + astrStoreProcedureName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        strSpDescription = reader.SafeGetString(1);
            }
            catch (Exception)
            {
                // ignored
            }
            return strSpDescription;
        }


        public static T SelectRow<T>(DataTable aDataTable) where T : new()
        {
            var lGetType = typeof(T); 
            var lInstanceOfType = new T();
            if (aDataTable.IsNotNull() && aDataTable.Rows.Count > 0)
            { 
                foreach (DataRow ldtDataRow in aDataTable.Rows)
                {
                   
                    foreach (DataColumn ldtDataColumn in aDataTable.Columns)
                    {
                        if (!Convert.IsDBNull(ldtDataRow[ldtDataColumn]))
                        {
                            var Pishare = lGetType.GetProperty(ldtDataColumn.ColumnName);
                            if (Pishare.IsNotNull())
                                Pishare.SetValue(lInstanceOfType, ldtDataRow[ldtDataColumn]);
                        }
                    } 
                    //execute first row and come out of loop.
                    break;
                }
            }

            return lInstanceOfType;
        }

        public static List<T> GetCollection<T>(DataTable aDataTable) where T : new()
        {
            var lGetType = typeof(T);
            List<T> lstTList = new List<T>();

            if (aDataTable.IsNotNull() && aDataTable.Rows.Count > 0)
            { 

                foreach (DataRow ldtDataRow in aDataTable.Rows)
                {
                    var lInstanceOfType = new T();
                    foreach (DataColumn ldtDataColumn in aDataTable.Columns)
                    {
                        if (!Convert.IsDBNull(ldtDataRow[ldtDataColumn]))
                        {
                            var Pishare = lGetType.GetProperty(ldtDataColumn.ColumnName);
                            if (Pishare.IsNotNull())
                                Pishare.SetValue(lInstanceOfType, ldtDataRow[ldtDataColumn]);
                        }
                    }
                    lstTList.Add(lInstanceOfType);
                }
            }

            return lstTList;
        }
        /// <summary>
        /// Get list of Index on the table
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableIndexInfo> GetTableIndexes(string astrTableName=null)
        {
            var lstTableIndexes = new List<TableIndexInfo>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand(); 
                command.CommandText = astrTableName.IsNullOrEmpty()? SqlQueryConstant.GetTablesIndex: SqlQueryConstant.GetTableIndex.Replace("@tblName", "'" + astrTableName + "'");
                DataTable ldtTableIndex = new DataTable();
                Database.OpenConnection();
                ldtTableIndex.Load(command.ExecuteReader());
                lstTableIndexes = GetCollection<TableIndexInfo>(ldtTableIndex);  
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableIndexes;
        }

        /// <summary>
        /// Get the table create script
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public TableCreateScript GetTableCreateScript(string astrTableName)
        {
            var lTableCreateScript = new TableCreateScript();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableCreateScript.Replace("@table_name", "'" + astrTableName + "'");
                Database.OpenConnection();
                DataTable ldtDataTable = new DataTable();
                ldtDataTable.Load(command.ExecuteReader());
                lTableCreateScript = SelectRow<TableCreateScript>(ldtDataTable); 
            }
            catch (Exception)
            {
                // ignored
            }

            return lTableCreateScript;
        }

        /// <summary>
        /// Get all the table related dependencies
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>

        public List<Tabledependencies> GetTableDependencies(string astrTableName)
        {
            var lstTableDependencies = new List<Tabledependencies>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableDependencies.Replace("@tblName", "'" + astrTableName + "'");
                Database.OpenConnection();
                DataTable ldtTableDependencies = new DataTable();
                ldtTableDependencies.Load(command.ExecuteReader());
                lstTableDependencies = GetCollection<Tabledependencies>(ldtTableDependencies);
            }
            catch (Exception)
            {
                // ignored
            }
            return lstTableDependencies.DistinctBy(x => x.name).ToList();
        }

        /// <summary>
        /// Get tables columns details
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableColumns> GetTablesColumn(string astrTableName=null)
        {
            var lstTablesColumn = new List<TableColumns>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = astrTableName.IsNullOrWhiteSpace() ? SqlQueryConstant.GetTablesColumn : SqlQueryConstant.GetTablesColumnWithTableName.Replace("@tblName", "'" + astrTableName + "'"); 
                Database.OpenConnection();
                DataTable ldtTableColumns = new DataTable(); 
                ldtTableColumns.Load(command.ExecuteReader());
                lstTablesColumn = GetCollection<TableColumns>(ldtTableColumns); 
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTablesColumn;
        }

        /// <summary>
        /// Get table related foreign keys
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableForeignKeys(string astrTableName)
        {
            var lstTableFkColumns = new List<TableFKDependency>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableForeignKeys.Replace("@tblName", "'" + astrTableName + "'");
                Database.OpenConnection();
                DataTable ldtTableFkColumn = new DataTable();
                ldtTableFkColumn.Load(command.ExecuteReader());
                lstTableFkColumns = GetCollection<TableFKDependency>(ldtTableFkColumn); 
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableFkColumns;
        }


        /// <summary>
        /// Get table Key constraints
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrTableName)
        {
            var lstTableKeyConstraints = new List<TableKeyConstraint>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableKeyConstraints.Replace("@tblName", "'" + astrTableName + "'");
                Database.OpenConnection(); 
                DataTable ldtKeyConstraints = new DataTable();
                ldtKeyConstraints.Load(command.ExecuteReader());
                lstTableKeyConstraints = GetCollection<TableKeyConstraint>(ldtKeyConstraints); 
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstTableKeyConstraints;
        }

        /// <summary>
        /// Get Current Database Name
        /// </summary>
        /// <returns></returns>

        public string GetCurrentDatabaseName()
        {
            return Database.GetDbConnection().Database;
            
        }
        /// <summary>
        /// Get tables with descriptions
        /// </summary>
        /// <returns></returns>
        public List<TablePropertyInfo> GetTablesDescription()
        {
            var lstTablesWithDescriptions = new List<TablePropertyInfo>();
            try
            { 
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTablesWithDescription;
                DataTable ldtTableWithDescriptions = new DataTable();
                Database.OpenConnection();
                ldtTableWithDescriptions.Load(command.ExecuteReader());
                lstTablesWithDescriptions = GetCollection<TablePropertyInfo>(ldtTableWithDescriptions);
            }
            catch (Exception ex)
            {
                // ignored
            }

            return lstTablesWithDescriptions;
        }

        //Get table descriptions
        public Ms_Description GetTableDescription(string astrTableName)
        {
            var lstrTableDescription = string.Empty;
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableDescription.Replace("@tblName", "'" + astrTableName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (!reader.HasRows) return new Ms_Description { desciption = lstrTableDescription };
                while (reader.Read())
                    lstrTableDescription = reader.SafeGetString(1);

                return new Ms_Description { desciption = lstrTableDescription };
            }
            catch (Exception)
            {
                return new Ms_Description { desciption = "" };
            }
        }

        /// <summary>
        /// Create or Update Table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        public void CreateOrUpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            try
            {
                UpdateTableDescription(astrDescriptionValue, astrSchemaName, astrTableName);
            }
            catch (Exception)
            {
                CreateTableDescription(astrDescriptionValue, astrSchemaName, astrTableName);
            }
        }

        /// <summary>
        /// Update the table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        private void UpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var tableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = SqlQueryConstant.UpdateTableExtendedProperty.Replace("@Table_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + tableName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        private void CreateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            var tableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = SqlQueryConstant.InsertTableExtendedProperty.Replace("@Table_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + tableName + "'");
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


        /// <summary>
        /// Create or update table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        public void CreateOrUpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            try
            {
                UpdateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnValue);
            }
            catch (Exception)
            {
                CreateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnValue);
            }
        }

        /// <summary>
        /// Update table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        private void UpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var lstrTableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = SqlQueryConstant.UpdateTableColumnExtendedProperty.Replace("@Column_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + lstrTableName + "'").Replace("@Column_Name", "'" + astrColumnValue + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        private void CreateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            var lstrTableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            command.CommandText = SqlQueryConstant.InsertTableColumnExtendedProperty.Replace("@Column_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + lstrTableName + "'").Replace("@Column_Name", "'" + astrColumnValue + "'");
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

        /// <summary>
        /// Get table Fragmentation details
        /// </summary>
        /// <returns></returns>
        public List<TableFragmentationDetails> GetTablesFragmentation()
        {
            var lstTableFragmentation = new List<TableFragmentationDetails>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.TableFragmentation;
                Database.OpenConnection();
                DataTable ldtTableFragmentationDetails = new DataTable();
                ldtTableFragmentationDetails.Load(command.ExecuteReader());
                lstTableFragmentation = GetCollection<TableFragmentationDetails>(ldtTableFragmentationDetails); 
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableFragmentation;//.Where(x => Convert.ToInt32(x.PercentFragmented) > 0).ToList();
        }

        /// <summary>
        /// Get table details
        /// </summary>
        /// <returns></returns>
        public List<string> GetTables()
        {
            var lstTables = new List<string>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTables;
                command.CommandType = CommandType.StoredProcedure;
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        if (!reader.GetString(1).Equals("sys") && reader.GetString(3).Equals("TABLE"))
                            //lstTables.Add( reader.GetString(2));
                            lstTables.Add(reader.GetString(1) + "." + reader.GetString(2));
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTables;
        }

        /// <summary>
        /// Get table columns
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<string> GetTableColumns(string astrTableName)
        {
            var lstTableColumns = new List<string>();
            try
            {
                using var command = Database.GetDbConnection().CreateCommand();
                command.CommandText = SqlQueryConstant.GetTableColumns.Replace("@tableName", astrTableName);
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstTableColumns.Add(reader.GetString(0));
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableColumns;
        }
        /// <summary>
        /// Get table dependencies
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableFkReferences(string astrSchemaName = null)
        {
            var lstTableFkDependencies = new List<TableFKDependency>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                if (astrSchemaName.IsNullOrEmpty())
                {
                    command.CommandText = SqlQueryConstant.GetTableFkReferences;
                }
                else
                {
                    command.CommandText = SqlQueryConstant.GetTableFkReferencesBySchemaName.Replace("@SchemaName", $"'{astrSchemaName}'");
                    
                }
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstTableFkDependencies.Add(new TableFKDependency
                        {
                            Fk_name = reader.GetString(0),
                            fk_refe_table_name = reader.GetString(1)
                        });
            }
            catch (Exception)
            {
                // ignored
            }
            return lstTableFkDependencies;
        }
        /// <summary>
        /// Get Database Triggers
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetTriggers()
        {
            var propertyInfos = new List<PropertyInfo>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetTriggers;
                Database.OpenConnection(); 
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        propertyInfos.Add(new PropertyInfo
                        {
                            istrName = reader.SafeGetString(0),
                            istrValue = reader.SafeGetString(1)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return propertyInfos;
        }

        /// <summary>
        /// Get Trigger Details by trigger name
        /// </summary>
        /// <param name="astrTriggerName"></param>
        /// <returns></returns>
        public List<TriggerInfo> GetTrigger(string astrTriggerName)
        {
            var triggerInfo = new List<TriggerInfo>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetTrigger.Replace("@TiggersName", "'" + astrTriggerName + "'");
                Database.OpenConnection();

                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        triggerInfo.Add(new TriggerInfo
                        {
                            TiggersName = reader.SafeGetString(0),
                            TiggersDesc = reader.SafeGetString(1),
                            TiggersCreateScript = reader.SafeGetString(2),
                            TiggersCreatedDate = reader.GetDateTime(3).ToString(CultureInfo.InvariantCulture),
                            TiggersModifyDate = reader.GetDateTime(4).ToString(CultureInfo.InvariantCulture)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return triggerInfo;
        }

        /// <summary>
        /// Create or update the trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrTriggerName"></param>
        public void CreateOrUpdateTriggerDescription(string astrDescriptionValue, string astrTriggerName)
        {
            try
            {
                UpdateTriggerDescription(astrDescriptionValue, astrTriggerName);
            }
            catch (Exception)
            {
                CreateTriggerDescription(astrDescriptionValue, astrTriggerName);
            }
        }

        /// <summary>
        /// Update Trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void UpdateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = SqlQueryConstant.UpdateTriggerExtendedProperty.Replace("@Trigger_value", "'" + astrDescriptionValue + "'").Replace("@Trigger_Name", "'" + astrSchemaName + "'");
            Database.OpenConnection();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create Trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void CreateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = SqlQueryConstant.CreateTriggerExtendedProperty.Replace("@Trigger_value", "'" + astrDescriptionValue + "'").Replace("@Trigger_Name", "'" + astrSchemaName + "'");
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
        /// <summary>
        /// Get list of UserDefined data types
        /// </summary>
        /// <returns></returns>
        public List<UserDefinedDataTypeDetails> GetUserDefinedDataTypes()
        {
            var lstUserDefinedDataTypeDetails = new List<UserDefinedDataTypeDetails>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetUserDefinedDataTypes;
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstUserDefinedDataTypeDetails.Add(new UserDefinedDataTypeDetails
                        {
                            name = reader.SafeGetString(0),
                            iblnallownull = reader.GetBoolean(1),
                            basetypename = reader.SafeGetString(2),
                            length = reader.GetInt16(3),
                            createscript = reader.SafeGetString(4)
                        });
            }
            catch
            {
                // ignored
            }

            return lstUserDefinedDataTypeDetails;
        }

        /// <summary>
        /// Get User defined data type by it names
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public UserDefinedDataTypeDetails GetUserDefinedDataType(string astrTypeName)
        {
            var lUserDefinedDataTypeDetails = new UserDefinedDataTypeDetails();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetUserDefinedDataTypeDetails.Replace("@TypeName", "'" + astrTypeName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lUserDefinedDataTypeDetails = new UserDefinedDataTypeDetails
                        {
                            name = reader.SafeGetString(0),
                            iblnallownull = reader.GetBoolean(1),
                            basetypename = reader.SafeGetString(2),
                            length = reader.GetInt16(3),
                            createscript = reader.SafeGetString(4)
                        };
            }
            catch (Exception)
            {
                // ignored
            }

            return lUserDefinedDataTypeDetails;
        }

        /// <summary>
        /// Get user defined data type references.
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReference(string astrTypeName)
        {
            var lstUserDefinedDataTypeReference = new List<UserDefinedDataTypeReferance>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetUsedDefinedDataTypeReference.Replace("@TypeName", "'" + astrTypeName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstUserDefinedDataTypeReference.Add(new UserDefinedDataTypeReferance
                        {
                            objectname = reader.SafeGetString(0),
                            typeofobject = reader.SafeGetString(1)
                        });
            }
            catch (Exception)
            {
                // ignored
            }
            return lstUserDefinedDataTypeReference;
        }

        /// <summary>
        ///Get  User defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string astrTypeName)
        {
            var lUserDefinedDataTypeDescription = new Ms_Description();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetUsedDefinedDataTypeExtendedProperties.Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lUserDefinedDataTypeDescription = new Ms_Description
                        {
                            desciption = reader.SafeGetString(0)
                        };
            }
            catch (Exception)
            {
                // ignored
            }

            return lUserDefinedDataTypeDescription;
        }

        /// <summary>
        /// Create User defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescValue"></param>
        private void CreateUsedDefinedDataTypeDescription(string astrTypeName, string astrDescValue)
        {
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.AddUserDefinedDataTypeExtendedProperty.Replace("@desc", "'" + astrDescValue + "'").Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // ignored
            }
        }
        /// <summary>
        /// Update user defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescValue"></param>
        private void UpdateUsedDefinedDataTypeDescription(string astrTypeName, string astrDescValue)
        {
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.UpdateUserDefinedDataTypeExtendedProperty.Replace("@desc", "'" + astrDescValue + "'").Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Create or update the user defined data type extended properties
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescriptionValue"></param>
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string astrTypeName, string astrDescriptionValue)
        {
            try
            {
                UpdateUsedDefinedDataTypeDescription(astrTypeName, astrDescriptionValue);
            }
            catch (Exception)
            {
                CreateUsedDefinedDataTypeDescription(astrTypeName, astrDescriptionValue);
            }
        }
        /// <summary>
        /// Get list of database views
        /// </summary>
        /// <returns></returns> 

        public List<PropertyInfo> GetViewsWithDescription()
        {
            var dbProperties = new List<PropertyInfo>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetViewsWithDescription; 
                Database.OpenConnection(); 
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        dbProperties.Add(new PropertyInfo
                        {
                            istrName = reader.SafeGetString(0),
                            istrValue = reader.SafeGetString(1)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return dbProperties;
        }

        /// <summary>
        /// Get view Dependencies
        /// </summary>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewDependancy> GetViewDependencies(string astrViewName)
        {
            var lstViewDependencies = new List<ViewDependancy>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetViewsDependancies.Replace("@viewname", "'" + astrViewName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstViewDependencies.Add(new ViewDependancy
                        {
                            name = reader.SafeGetString(0)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstViewDependencies;
        }

        /// <summary>
        /// Get view Properties
        /// </summary>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<View_Properties> GetViewProperties(string astrViewName)
        {
            var lstViewProperties = new List<View_Properties>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetViewProperties.Replace("@viewname", "'" + astrViewName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstViewProperties.Add(new View_Properties
                        {
                            uses_ansi_nulls = reader.SafeGetString(0),
                            uses_quoted_identifier = reader.SafeGetString(1),
                            create_date = reader.SafeGetString(2),
                            modify_date = reader.SafeGetString(3)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstViewProperties;
        }
        /// <summary>
        /// Get view column details
        /// </summary>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewColumns> GetViewColumns(string astrViewName)
        {
            var lstGetViewColumns = new List<ViewColumns>();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetViewColumns.Replace("@viewname", "'" + astrViewName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lstGetViewColumns.Add(new ViewColumns
                        {
                            name = reader.SafeGetString(0),
                            type = reader.SafeGetString(1),
                            updated = reader.SafeGetString(2),
                            selected = reader.SafeGetString(3),
                            column_name = reader.SafeGetString(4)
                        });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstGetViewColumns;
        }
        /// <summary>
        /// Get view create script
        /// </summary>
        /// <param name="astrViewName"></param>
        /// <returns></returns>

        public ViewCreateScript GetViewCreateScript(string astrViewName)
        {
            var lViewCreateScript = new ViewCreateScript();
            try
            {
                using var lDbConnection = Database.GetDbConnection();
                var command = lDbConnection.CreateCommand();
                command.CommandText = SqlQueryConstant.GetViewCreateScript.Replace("@viewname", "'" + astrViewName + "'");
                Database.OpenConnection();
                using var reader = command.ExecuteReader();
                if (reader.HasRows)
                    while (reader.Read())
                        lViewCreateScript.createViewScript = reader.SafeGetString(0);
            }
            catch (Exception)
            {
                // ignored
            }

            return lViewCreateScript;
        }

        /// <summary>
        /// Get Object dependent On
        /// </summary>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public List<ReferencesModel> GetObjectThatDependsOn(string astrObjectName)
        {
            var lstObjectDependsOn = new List<ReferencesModel>(); 
                try
                {
                    using var lDbConnection = Database.GetDbConnection();
                    var command = lDbConnection.CreateCommand();
                    var newObjectName = astrObjectName.Replace(astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                    command.CommandText = SqlQueryConstant.ObjectThatDependsOn.Replace("@ObjectName", "'" + newObjectName + "'");
                    Database.OpenConnection();
                    DataTable ldtObjectDependsOn = new DataTable();
                    ldtObjectDependsOn.Load(command.ExecuteReader());
                    lstObjectDependsOn = GetCollection<ReferencesModel>(ldtObjectDependsOn); 
                }
                catch (Exception ex)
                {
                    // ignored
                } 
            return lstObjectDependsOn
;
        }

        /// <summary>
        /// Get Object which dependent on
        /// </summary>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public List<ReferencesModel> GetObjectOnWhichDepends(string astrObjectName)
        {
            var lstObjectOnWhichDepends = new List<ReferencesModel>();
              
                try
                {
                    using var lDbConnection = Database.GetDbConnection();
                          var command = lDbConnection.CreateCommand();
                    var newObjectName = astrObjectName.Replace(astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                    command.CommandText = SqlQueryConstant.ObjectOnWhichDepends.Replace("@ObjectName", "'" + newObjectName + "'");
                    Database.OpenConnection(); 
                    DataTable ldtObjectOnWhichDepends = new DataTable();
                    ldtObjectOnWhichDepends.Load(command.ExecuteReader());
                    lstObjectOnWhichDepends = GetCollection<ReferencesModel>(ldtObjectOnWhichDepends);
                     
                }
                catch (Exception)
                {
                    // ignored
                } 

            return lstObjectOnWhichDepends;
        }
    }
}