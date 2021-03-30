using MSSQL.DIARY.COMN.Enums;
using System.Collections.Generic;

namespace MSSQL.DIARY.COMN.Models
{
    public class TreeViewJson
    {
        public string text { get; set; }
        public string icon { get; set; }
        public string mdaIcon { get; set; }
        public string link { get; set; }
        public bool selected { get; set; }
        public int badge { get; set; }
        public bool expand { get; set; }
        public bool leaf { get; set; }
        public SchemaEnums SchemaEnums { get; set; }
        public IList<TreeViewJson> children { get; set; }
    }
}