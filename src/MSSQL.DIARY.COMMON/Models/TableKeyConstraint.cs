namespace MSSQL.DIARY.COMN.Models
{
    public class TableKeyConstraint
    {
        public string table_view { get; set; }
        public string object_type { get; set; }
        public string Constraint_type { get; set; }
        public string Constraint_name { get; set; }
        public string Constraint_details { get; set; }
    }
}