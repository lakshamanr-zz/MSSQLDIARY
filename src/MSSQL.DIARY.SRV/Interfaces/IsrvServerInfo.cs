using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.SRV.Interfaces
{
    public interface ISrvServerInfo
    {
        List<PropertyInfo> GetAdvancedServerSettingsInfo();
        List<string> GetDatabaseNames();
        List<string> GetServerName();
        List<PropertyInfo> GetServerProperties();
    }
}