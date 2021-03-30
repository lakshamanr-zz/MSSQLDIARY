using System.Collections.Generic;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseWorkFlow
    {
        public List<string> GetWorkList(string istrdbConn)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetWorkList();
            }
        }

        public List<string> BuildBusinessWorkFlowTree(string istrdbConn, string istrWorkFlowName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetWorkDetailsbyName(istrWorkFlowName);
            }
        }

        public string GetWorkDetailsbyName(string istrdbConn, string istrWorkFlowName)
        {
            var srvDatabaseObjectDependncy = new SrvDatabaseObjectDependncy();
            return srvDatabaseObjectDependncy.WorkFlowJsonResutl(
                srvDatabaseObjectDependncy
                    .GetBusinessWorkFlowJson(
                        BuildBusinessWorkFlowTree(
                            istrdbConn, istrWorkFlowName)), istrWorkFlowName);
        }
    }
}