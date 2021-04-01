using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public string GetServerName()
        {
            var lstrServerName = "";
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetServerName;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstrServerName = reader.GetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstrServerName;
        }

        public List<string> GetTables()
        {
            var lstTables = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTables;
                    command.CommandType = CommandType.StoredProcedure;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                if (!reader.GetString(1).Equals("sys") && reader.GetString(3).Equals("TABLE"))
                                    //lstTables.Add( reader.GetString(2));
                                    lstTables.Add(reader.GetString(1) + "." + reader.GetString(2));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTables;
        }

        public List<string> GetViews()
        {
            var lstTables = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAllViewsDetailsWithMsDesc;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTables.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTables;
        }

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

        public List<string> GetTriggers()
        {
            var lstrTriggers = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTigger;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstrTriggers.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstrTriggers;
        }

        public List<string> GetScalarFunctions()
        {
            var lstScalarFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetScalarFunctions;
                    command.CommandTimeout = 10 * 60;
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

        public List<string> GetTableValueFunctions()
        {
            var lstTableValueFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableValueFunctions;
                    command.CommandTimeout = 10 * 60;
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

        public List<string> GetAggregateFunctions()
        {
            var lstAggregateFunctions = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAggregateFunctions;
                    command.CommandTimeout = 10 * 60;
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

        public List<string> GetUserDefinedDataTypes()
        {
            var lstUserDefinedDataType = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetUserDefinedDataType;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstUserDefinedDataType.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return lstUserDefinedDataType;
        }

        public List<string> GetXmlSchemas()
        {
            var lstXmlSchemas = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetXmlSchemas;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstXmlSchemas.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return lstXmlSchemas;
        }

        public List<PropertyInfo> GetServerProperties()
        {
            var lstServerProperties = new List<PropertyInfo>();
            var count = 0;
            try
            {
                foreach (var sqlQuery in SqlQueryConstant.GetServerProperties)
                {
                    using (var command = Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = SqlQueryConstant.GetServerProperties[count];
                        command.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstServerProperties.Add(new PropertyInfo
                                    {
                                        istrName = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName)
                                            .ToList().FirstOrDefault(),
                                        istrValue = reader.GetString(0).Replace("\0", "")
                                    });
                        }
                    }

                    count++;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstServerProperties;
        }

        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            var lstAdvancedServerSettings = new List<PropertyInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAdvancedServerSettings;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstAdvancedServerSettings.Add(new PropertyInfo
                                {
                                    istrName = reader.GetString(0),
                                    istrValue = reader.GetString(1).Replace("\0", "")
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstAdvancedServerSettings;
        }

        public List<string> GetDatabaseNames()
        {
            var lstDatabaseNames = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetDatabaseNames;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstDatabaseNames.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstDatabaseNames;
        }

        public List<string> GetTableColumns(string istrTableName)
        {
            var lstTableColumns = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableColumns.Replace("@tableName", istrTableName);

                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableColumns.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableColumns;
        }
    }
}