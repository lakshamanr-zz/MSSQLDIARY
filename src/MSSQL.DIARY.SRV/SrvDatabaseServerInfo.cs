using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;
using MSSQL.DIARY.SRV.Interfaces;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseServerInfo  
    {

        public string istrDBConnection { get; set; }
        public List<string> GetServerName()
        {
            return GetServerNameList();
        }

        public List<string> GetDatabaseNames()
        {
            return GetDatabaseNameList();
        }

        public List<PropertyInfo> GetServerProperties()
        {
            return GetServerPropertiesList();
        }

        public List<PropertyInfo> GetAdvancedServerSettingsInfo()
        {
            return GetAdvancedServerSettingsInfoList();
        }

        public static List<string> GetServerNameList()
        {
            var lst = new List<string>();
            using (var dbSqldocContext = new MsSqlDiaryContext())
            {
                lst.Add(dbSqldocContext.GetServerName());
            }

            return lst;
        }

        private  List<string> GetDatabaseNameList()
        {
            var lst = new List<string>();
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetDatabaseNames();
            }
        }

        private   List<PropertyInfo> GetServerPropertiesList()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetServerProperties();
            }
        }

        private   List<PropertyInfo> GetAdvancedServerSettingsInfoList()
        {
            var lst = new List<PropertyInfo>();
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetAdvancedServerSettings();
            }
        }
    }
}