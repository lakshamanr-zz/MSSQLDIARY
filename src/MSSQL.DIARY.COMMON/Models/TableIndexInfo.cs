namespace MSSQL.DIARY.COMN.Models
{
    public class TableIndexInfo
    {
        public string index_name { get; set; }
        public string columns { get; set; }
        public string index_type { get; set; }
        public string unique { get; set; }
        public string tableView { get; set; }
        public string object_Type { get; set; }
    }
}