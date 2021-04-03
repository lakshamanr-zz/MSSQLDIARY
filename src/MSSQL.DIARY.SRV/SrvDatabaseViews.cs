using System.Collections.Generic;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseViews
    {
        public static NaiveCache<List<PropertyInfo>> CacheViewDetails = new NaiveCache<List<PropertyInfo>>();

        /// <summary>
        /// Get Views Details with it Description
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetViewsWithDescription(string astrDatabaseName)
        {
            return CacheViewDetails.GetOrCreate(astrDatabaseName + "ViewsWithDescription", () => CacheViewsWithDescription(astrDatabaseName));
        }
        /// <summary>
        /// Cache view details 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        private static List<PropertyInfo> CacheViewsWithDescription(string astrDatabaseName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewsWithDescription();
            }
        }
        /// <summary>
        /// Get View Dependencies
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewDependancy> GetViewDependencies(string astrDatabaseName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewDependencies(astrViewName);
            }
        }
        /// <summary>
        /// Get view Properties
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<View_Properties> GetViewProperties(string astrDatabaseName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewProperties(astrViewName);
            }
        }
        /// <summary>
        /// Get view Columns 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewColumns> GetViewColumns(string astrDatabaseName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewColumns(astrViewName);
            }
        }
        /// <summary>
        /// Get view Create script
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public ViewCreateScript GetViewCreateScript(string astrDatabaseName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewCreateScript(astrViewName);
            }
        }
        /// <summary>
        /// Get view Detail
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>

        public PropertyInfo GetViewsWithDescription(string astrDatabaseName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetViewsWithDescription().Find(x => x.istrName.Contains(astrViewName));
            }
        }
    }
}