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
        public string GetDatabaseServerName()
        {
            var istrDataBaseServerName = "";
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetDatabaseServerName;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                istrDataBaseServerName = reader.GetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return istrDataBaseServerName;
        }

        public List<string> GetTables()
        {
            var lstTables = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetTables;
                    commad.CommandType = CommandType.StoredProcedure;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAllViewsDetailsWithMsDesc;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetStoreProcedures;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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

        public List<string> GetTigger()
        {
            var tigger = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetTigger;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tigger.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tigger;
        }

        public List<string> GetScalarFunction()
        {
            var scalarFunction = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetScalarFunctions;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                scalarFunction.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return scalarFunction;
        }

        public List<string> GetTableValueFunction()
        {
            var tableValueFunction = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetTableValueFunctions;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableValueFunction.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return tableValueFunction;
        }

        public List<string> GetAggregateFunctions()
        {
            var aggregateFunctions = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAggregateFunctions;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                aggregateFunctions.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return aggregateFunctions;
        }

        public List<string> GetUserDefinedDataType()
        {
            var userDefinedDataType = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetUserDefinedDataType;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                userDefinedDataType.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }


            return userDefinedDataType;
        }

        public List<string> GetXmlSchemaCollection()
        {
            var xmlSchemaCollection = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetXmlSchemaCollection;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                xmlSchemaCollection.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
            }


            return xmlSchemaCollection;
        }

        public List<PropertyInfo> GetServerProperties()
        {
            var serverPropericesInfos = new List<PropertyInfo>();
            var count = 0;
            try
            {
                foreach (var sqlQuery in SqlQueryConstant.GetServerProperties)
                {
                    using (var commad = Database.GetDbConnection().CreateCommand())
                    {
                        commad.CommandText = SqlQueryConstant.GetServerProperties[count];
                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = commad.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    serverPropericesInfos.Add(new PropertyInfo
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

            return serverPropericesInfos;
        }

        public List<PropertyInfo> GetAdvancedServerSettingsInfo()
        {
            var advancedServerSettingsInfo = new List<PropertyInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAdvancedServerSettings;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                advancedServerSettingsInfo.Add(new PropertyInfo
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

            return advancedServerSettingsInfo;
        }

        public List<string> GetDatabaseNames()
        {
            var lstOfDatabases = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAllDatabaseNames;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstOfDatabases.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstOfDatabases;
        }

        public List<string> GetTableColumns(string istrTableName)
        {
            var lstTableColumns = new List<string>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetTableColumns.Replace("@tableName", istrTableName);

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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