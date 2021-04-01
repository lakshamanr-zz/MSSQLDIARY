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
        public List<ReferencesModel> GetObjectThatDependsOn(string astrObjectName)
        {
            var listOfObjectDependncy = new List<ReferencesModel>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    try
                    {
                        var commad = conn.CreateCommand();
                        var newObjectName = astrObjectName.Replace(
                            astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".",
                            "");
                        commad.CommandText =
                            SqlQueryConstant.ObjectThatDependsOn.Replace("@ObjectName", "'" + newObjectName + "'");
                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = commad.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    listOfObjectDependncy.Add(new ReferencesModel
                                    {
                                        ThePath = reader.SafeGetString(0),
                                        TheFullEntityName = reader.SafeGetString(1),
                                        TheType = reader.SafeGetString(2),
                                        iteration = reader.GetInt32(3)
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

            return listOfObjectDependncy;
        }

        public List<ReferencesModel> GetObjectOnWhichDepends(string astrObjectName)
        {
            var listOfObjectDependncy = new List<ReferencesModel>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    try
                    {
                        var commad = conn.CreateCommand();
                        var newObjectName = astrObjectName.Replace(
                            astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".",
                            "");
                        commad.CommandText =
                            SqlQueryConstant.ObjectOnWhichDepends.Replace("@ObjectName", "'" + newObjectName + "'");
                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = commad.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    listOfObjectDependncy.Add(new ReferencesModel
                                    {
                                        ThePath = reader.SafeGetString(0),
                                        TheFullEntityName = reader.SafeGetString(1),
                                        TheType = reader.SafeGetString(2),
                                        iteration = reader.GetInt32(3)
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

            return listOfObjectDependncy;
        }
    }
}