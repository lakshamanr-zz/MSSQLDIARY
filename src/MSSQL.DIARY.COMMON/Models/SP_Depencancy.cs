namespace MSSQL.DIARY.COMN.Models
{
    public class SP_Depencancy
    {
        public string referencing_object_name { get; set; }
        public string referencing_object_type { get; set; }
        public string referenced_object_name { get; set; }
    }
}