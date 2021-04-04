using System;
using System.Collections.Generic;
using System.Text;

namespace MSSQL.DIARY.COMN.Models
{
    public class TableWithSchema
    {
        public Dictionary<string, Dictionary<string, string>> keyValuePairs { get; set; }
        public string istrSchemaName { get; set; }
    }
}
