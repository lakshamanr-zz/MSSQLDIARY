namespace MSSQL.DIARY.COMN.Models
{
    public class ExecutionPlanInfo
    {
        public string QueryPlanXML { get; set; }
        public string UseAccounts { get; set; }
        public string CacheObjectType { get; set; }
        public string Size_In_Byte { get; set; }
        public string SqlText { get; set; }
    }
}