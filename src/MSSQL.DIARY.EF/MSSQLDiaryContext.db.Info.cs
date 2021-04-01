using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        protected MssqlDiaryContext()
        {
        }

        public MssqlDiaryContext(DbContextOptions options) : base(options)
        {
        }

        public List<PropertyInfo> GetdbProperties()
        {
            var dbProperties = new List<PropertyInfo>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText =
                        SqlQueryConstant.GetdbProperties.Replace("@DatabaseName", $"'{conn.Database}'");
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                for (var i = 0; i < 12; i++)
                                    dbProperties.Add(new PropertyInfo
                                    {
                                        istrName = reader.GetName(i),
                                        istrValue = reader.GetString(i)
                                    });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return dbProperties;
        }

        public List<PropertyInfo> GetdbOptions()
        {
            var dbOptions = new List<PropertyInfo>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetdbOptions.Replace("@DatabaseName", $"'{conn.Database}'");
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                for (var i = 0; i < 14; i++)
                                    dbOptions.Add(new PropertyInfo
                                    {
                                        istrName = reader.GetName(i),
                                        istrValue = reader.GetString(i)
                                    });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return dbOptions;
        }

        public List<FileInfomration> GetdbFiles()
        {
            var dbFile = new List<FileInfomration>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetdbFiles.Replace("@DatabaseName", $"'{conn.Database}'");
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                dbFile.Add(new FileInfomration
                                {
                                    Name = reader.GetString(0),
                                    FileType = reader.GetString(1),
                                    FileLocation = reader.GetString(2),
                                    FileSize = reader.GetInt32(3).ToString()
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return dbFile;
        }

        public List<TableFKDependency> GetTableReferences(string istrSchemaName = null)
        {
            var tableFkDependencies = new List<TableFKDependency>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    if (istrSchemaName.IsNullOrEmpty())
                    {
                        commad.CommandText = SqlQueryConstant.AllDatabaseReferances;
                    }
                    else
                    {
                        commad.CommandText =
                            SqlQueryConstant.AllDatabaseReferancesBySchemaName.Replace("@SchemaName",
                                $"'{istrSchemaName}'");
                        ;
                    }

                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableFkDependencies.Add(new TableFKDependency
                                {
                                    Fk_name = reader.GetString(0),
                                    fk_refe_table_name = reader.GetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tableFkDependencies;
        }
    }
}