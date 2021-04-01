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
        /// Get Object dependent On
        /// </summary>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public List<ReferencesModel> GetObjectThatDependsOn(string astrObjectName)
        {
            var lstObjectDependsOn = new List<ReferencesModel>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    try
                    {
                        var command = lDbConnection.CreateCommand();
                        var newObjectName = astrObjectName.Replace(astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                        command.CommandText = SqlQueryConstant.ObjectThatDependsOn.Replace("@ObjectName", "'" + newObjectName + "'"); 
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstObjectDependsOn
.Add(new ReferencesModel
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
                using (var lDbConnection = Database.GetDbConnection())
                {
                    try
                    {
                        var command = lDbConnection.CreateCommand();
                        var newObjectName = astrObjectName.Replace(astrObjectName.Substring(0, astrObjectName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                        command.CommandText = SqlQueryConstant.ObjectOnWhichDepends.Replace("@ObjectName", "'" + newObjectName + "'"); 
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstObjectOnWhichDepends.Add(new ReferencesModel
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

            return lstObjectOnWhichDepends;
        }
    }
}