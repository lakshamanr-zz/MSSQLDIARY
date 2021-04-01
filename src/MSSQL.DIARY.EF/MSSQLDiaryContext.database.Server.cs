using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
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

        /// <summary>
        /// Get xml schemas
        /// </summary>
        /// <returns></returns>

        public List<string> GetXmlSchemas()
        {
            var lstXmlSchemas = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetXmlSchemas;
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
                    using (var command = Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = SqlQueryConstant.GetServerProperties[count];
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstServerProperties.Add(new PropertyInfo
                                    {
                                        istrName = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).FirstOrDefault(),
                                        istrValue = reader.GetString(0).Replace("\0", "")
                                    });
                        }
                    }
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
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAdvancedServerSettings;
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