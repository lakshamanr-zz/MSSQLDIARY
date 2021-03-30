using System.Collections.Generic;

namespace MSSQL.DIARY.COMN.Models
{
    public class SP_PropertyInfo
    {
        public string istrName { get; set; }
        public string istrValue { get; set; }

        public List<string> lstrCreateScript { get; set; }
        public List<string> lstSSISpackageReferance { get; set; }
    }
}