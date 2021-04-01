using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext  
    {

        /// <summary>
        /// Get list of database views
        /// </summary>
        /// <returns></returns> 

        public List<PropertyInfo> GetViewsWithDescription()
        {
            var dbProperties = new List<PropertyInfo>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetViewsWithDescription;
                    command.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                dbProperties.Add(new PropertyInfo
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand(); 
                    command.CommandText = SqlQueryConstant.GetViewsdependancies.Replace("@viewname", "'" + astrViewName + "'");
                    Database.OpenConnection(); 
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstViewDependencies .Add(new ViewDependancy
                                {
                                    name = reader.SafeGetString(0)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstViewDependencies ;
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetViewProperties.Replace("@viewname", "'" + astrViewName + "'"); 
                    Database.OpenConnection(); 
                    using (var reader = command.ExecuteReader())
                    {
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
                }
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                     var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetViewColumns.Replace("@viewname", "'" + astrViewName + "'"); 
                    Database.OpenConnection(); 
                    using (var reader = command.ExecuteReader())
                    {
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
                }
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();  
                    command.CommandText = SqlQueryConstant.GetViewCreateScript.Replace("@viewname", "'" + astrViewName + "'"); 
                    Database.OpenConnection(); 
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lViewCreateScript.createViewScript = reader.SafeGetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lViewCreateScript;
        }
    }
}