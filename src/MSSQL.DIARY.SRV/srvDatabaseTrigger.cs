using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class srvDatabaseTrigger
    {
        public List<PropertyInfo> GetAllDatabaseTrigger(string istrdbName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetTriggers();
            }
        }

        public List<TriggerInfo> GetTriggerInfosByName(string istrdbName, string istrTriggerName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetTrigger(istrTriggerName);
            }
        }

        public void CreateOrUpdateTriggerDescription(string istrdbName, string astrDescription_Value,
            string astrTrigger_Name)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                dbSqldocContext.CreateOrUpdateTriggerDescription(astrDescription_Value, astrTrigger_Name);
            }
        }
    }
}