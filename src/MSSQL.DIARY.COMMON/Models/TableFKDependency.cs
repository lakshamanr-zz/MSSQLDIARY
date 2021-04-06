namespace MSSQL.DIARY.COMN.Models
{
    public class TableFKDependency
    {
        public string values { get; set; }
        public string current_table_name { get; set; }
        public string Fk_name { get; set; }
        public string current_table_fk_columnName { get; set; }
        public string fk_refe_table_name { get; set; }
        public string fk_ref_table_column_name { get; set; }
        public string schema_name { get; set; }
    }
}