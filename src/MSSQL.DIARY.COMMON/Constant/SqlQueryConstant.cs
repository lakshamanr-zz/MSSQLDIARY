namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static string CreateWorkFlowTableIfNotExistis = @"CREATE TABLE dbo.WorkFlowName (	WorkFlowName varchar(100), 	ThePath varchar(4000), 	FullEntityName varchar(100),  )";
        public static string GetWorkList = @"select Distinct WorkFlowName from dbo.WorkFlowName ";
        public static string GetWorkFlowDetailsbyName = @"select ThePath  from dbo.WorkFlowName where WorkFlowName=@WorkFlowName";
    }
}
