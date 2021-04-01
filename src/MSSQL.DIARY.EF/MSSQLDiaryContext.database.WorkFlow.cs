using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public List<string> GetWorkList()
        {
            var lstWorkflows = new List<string>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetWorkList;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstWorkflows.Add(reader.SafeGetString(0));
                    }
                }
            }
            catch (Exception)
            {
                //igonre
            }

            return lstWorkflows;
        }

        public List<string> GetWorkDetailsbyName(string astrWorkFlowName)
        {
            var lstWorkFlowList = new List<string>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText =
                        SqlQueryConstant.GetWorkFlowDetailsbyName.Replace("@WorkFlowName",
                            "'" + astrWorkFlowName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstWorkFlowList.Add(reader.SafeGetString(0));
                    }
                }
            }
            catch (Exception)
            {
                //igonre
            }

            return lstWorkFlowList;
        }
    }
}