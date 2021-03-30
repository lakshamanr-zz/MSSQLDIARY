using System.Collections.Generic;

namespace MSSQL.DIARY.COMN.Models
{
    public class TablePropertyInfo
    {
        public string istrName { get; set; }
        public string istrFullName { get; set; }
        public string istrSchemaName { get; set; }
        public string istrValue { get; set; }
        public List<TableColumns> tableColumns { get; set; }
        public List<string> lstSSISpackageReferance { get; set; }
        public string id { get; set; }
        public string itemName { get; set; }
        public string istrNevigation { get; set; }

    }
}
