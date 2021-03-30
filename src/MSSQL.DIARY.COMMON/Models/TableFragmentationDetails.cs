namespace MSSQL.DIARY.COMN.Models
{
    public class TableFragmentationDetails
    {
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public string PercentFragmented { get; set; }
    }
}
