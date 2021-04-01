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

        /// <summary>
        /// Get server name.
        /// </summary>
        /// <returns></returns>
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

       

      
    }
}