using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext  
    {
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

        public List<PropertyInfo> GetAllViewsDetailsWithms_description()
        {
            var dbProperties = new List<PropertyInfo>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetAllViewsDetailsWithMsDesc;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
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

        public List<ViewDependancy> GetViewDependancies(string astrViewName)
        {
            var lstViewdependancy = new List<ViewDependancy>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    //var newViewName = astrViewName.Replace(astrViewName.Substring(0, astrViewName.IndexOf(".")) + ".", "");
                    commad.CommandText =
                        SqlQueryConstant.GetViewsdependancies.Replace("@viewname", "'" + astrViewName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstViewdependancy.Add(new ViewDependancy
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

            return lstViewdependancy;
        }

        public List<View_Properties> GetViewProperties(string astrViewName)
        {
            var lstViewProperties = new List<View_Properties>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText =
                        SqlQueryConstant.GetViewProperties.Replace("@viewname", "'" + astrViewName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();


                    using (var reader = commad.ExecuteReader())
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

        public List<ViewColumns> GetViewColumns(string astrViewName)
        {
            var lstGetViewColumns = new List<ViewColumns>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                     var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetViewColumns.Replace("@viewname", "'" + astrViewName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
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

        public ViewCreateScript GetViewCreateScript(string astrViewName)
        {
            var createScript = new ViewCreateScript();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    //var newViewName = astrViewName.Replace(astrViewName.Substring(0, astrViewName.IndexOf(".")) + ".", "");

                    commad.CommandText =
                        SqlQueryConstant.GetViewCreateScript.Replace("@viewname", "'" + astrViewName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                createScript.createViewScript = reader.SafeGetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return createScript;
        }
    }
}