namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static readonly string GetViewsWithDescription = @"SELECT   ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])AS 'STOREPROC' , ep.value AS [Extended property] FROM sys.extended_properties EP LEFT JOIN SYS.OBJECTS O ON ep.major_id = O.object_id  WHERE O.TYPE='V' ";

        public static readonly string GetViewProperties = @"SELECT 	CAST(uses_ansi_nulls as VARCHAR(1)) as uses_ansi_nulls	,CAST(uses_quoted_identifier as VARCHAR(1)) as uses_quoted_identifier ,CAST( create_date as varchar(100)) as create_date,   CAST(modify_date as varchar(100)) as modify_date FROM  sys.views vs inner join sys.sql_modules   sqlM ON vs.object_id=sqlM.object_id where sqlM.object_id=OBJECT_ID(@viewname)";

        public static readonly string GetViewColumns = @"declare @Table table ([name] varchar(100),[type] varchar(1000),updated varchar(100),selected varchar(100),column_name varchar(1000))INSERT INTO @Table exec sp_depends @viewname select * from @Table";

        public static readonly string GetViewCreateScript = @"select sqlM.definition FROM  sys.views vs inner join sys.sql_modules   sqlM ON vs.object_id=sqlM.object_id where sqlM.object_id=OBJECT_ID(@viewname)";

        public static readonly string GetViewsDependancies = @"declare @Table table ([name] varchar(100),[type] varchar(1000),updated varchar(100),selected varchar(100),column_name varchar(1000))INSERT INTO @Table exec sp_depends @viewnameselect DISTINCT name from @Table";
    }
}