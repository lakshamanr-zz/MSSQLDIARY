namespace MSSQL.DIARY.COMN.Models
{
    public class FunctionParameters
    {
        public string name { get; set; }
        public string type { get; set; }
        public string updated { get; set; }
        public string selected { get; set; }
        public string column_name { get; set; }
        public  string max_length { get; set; }
        public string Prec { get; set; }
        public string Scale { get; set; }
        public int Param_order { get; set; }
        public string Collection { get; set; }
        public string Extendedproperty { get; set; }
    }
}