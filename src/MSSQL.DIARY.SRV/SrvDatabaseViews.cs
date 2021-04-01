using System.Collections.Generic;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseViews
    {
        public static NaiveCache<List<PropertyInfo>> cacheViewDetails = new NaiveCache<List<PropertyInfo>>();

        public List<PropertyInfo> GetAllViewsDetailsWithms_description(string istrdbName)
        {
            return cacheViewDetails.GetOrCreate(istrdbName + "ViewDetails", () => CacheViewDetails(istrdbName));
        }

        private static List<PropertyInfo> CacheViewDetails(string istrdbName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetAllViewsDetailsWithms_description();
            }
        }

        public List<ViewDependancy> GetView_Dependancies(string istrdbName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetViewDependancies(astrViewName);
            }
        }

        public List<View_Properties> GetViewProperties(string istrdbName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetViewProperties(astrViewName);
            }
        }

        public List<ViewColumns> GetViewColumns(string istrdbName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetViewColumns(astrViewName);
            }
        }

        public ViewCreateScript GetViewCreateScript(string istrdbName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetViewCreateScript(astrViewName);
            }
        }

        public PropertyInfo GetViewNameWithMs_description(string istrdbName, string astrViewName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetAllViewsDetailsWithms_description()
                    .Find(x => x.istrName.Contains(astrViewName));
            }
        }
    }
}