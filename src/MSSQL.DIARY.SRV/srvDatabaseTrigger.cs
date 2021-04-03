using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseTrigger
    {
        /// <summary>
        /// Get Database Triggers
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetTriggers(string astrDatabaseName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetTriggers();
            }
        }

        /// <summary>
        /// Get Trigger Details by trigger name
        /// </summary>
        /// <returns></returns>
        public List<TriggerInfo> GetTrigger(string astrDatabaseName, string istrTriggerName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetTrigger(istrTriggerName);
            }
        }

        /// <summary>
        /// Create or update the trigger descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrTriggerName"></param>
        public void CreateOrUpdateTriggerDescription(string astrDatabaseName, string astrDescriptionValue, string astrTriggerName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                dbSqldocContext.CreateOrUpdateTriggerDescription(astrDescriptionValue, astrTriggerName);
            }
        }
    }
}