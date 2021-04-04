using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;
using System;
using System.Collections.Generic;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetDatabaseProperties.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                for (var i = 0; i < 12; i++)
                                    lstDatabaseProperties.Add(new PropertyInfo
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetDatabaseOptions.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                for (var i = 0; i < 14; i++)
                                    lstDatabaseOptions.Add(new PropertyInfo
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetDatabaseFiles.Replace("@DatabaseName", $"'{lDbConnection.Database}'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstDatabaseFiles.Add(new FileInfomration
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

            return lstDatabaseFiles;
        }
        
        /// <summary>
        /// Get database names.
        /// </summary>
        /// <returns></returns>
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
    }
}