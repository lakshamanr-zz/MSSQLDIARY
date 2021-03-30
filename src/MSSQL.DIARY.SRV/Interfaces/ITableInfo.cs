using System.Collections.Generic;

namespace MSSQL.DIARY.SRV.Interfaces
{
    public interface ITableInfo
    {
        List<string> GetTableNames();
    }
}