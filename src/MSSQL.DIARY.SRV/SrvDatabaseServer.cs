using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseServer  :SrvMain
    { 
        /// <summary>
        /// Get server names
        /// </summary>
        /// <returns></returns>
        public List<string> GetServerName()
        {
            var lstServers = new List<string>();
            using (var dbSqldocContext = new MsSqlDiaryContext())
            {
                lstServers.Add(dbSqldocContext.GetServerName());
            } 
            return lstServers;
        }
        /// <summary>
        /// Get database names.
        /// </summary>
        /// <returns></returns>
        public List<string> GetDatabaseNames()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetDatabaseNames();
            }
        }
        /// <summary>
        /// Get server Properties
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetServerProperties()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetServerProperties();
            }
        }
        /// <summary>
        /// Get server advance properties.
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetAdvancedServerSettings();
            }
        } 
        
    }
}