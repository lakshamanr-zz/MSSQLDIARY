namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static readonly string GetCurrentDataBaseName = @"SELECT DB_NAME() AS [Current_Database]";
        public static readonly string GetDatabaseNames = @"SELECT name FROM master.dbo.sysdatabases where name not in ('master','tempdb','model','msdb')";

        public static readonly string GetServerName = @"SELECT @@SERVERNAME";

        public static readonly string GetTables = @"[sys].[sp_tables]";

        public static readonly string GetTableColumns = @"SELECT  c.name AS column_name FROM sys.tables AS t INNER JOIN sys.columns c ON t.OBJECT_ID = c.OBJECT_ID WHERE t.name LIKE '%@tableName%' ";

        public static readonly string GetStoreProcedures = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ [NAME])AS 'STOREPROC' FROM SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID
                                                              WHERE O.TYPE='P'";

        public static readonly string GetTigger = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ [NAME])AS 'Tiggers' FROM SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID
                                                              WHERE O.TYPE='TR'";

        public static readonly string GetScalarFunctions = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ [NAME])AS 'SQL_SCALAR_FUNCTION' FROM SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID
                                                              WHERE O.TYPE='FN'";

        public static readonly string GetTableValueFunctions = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ [NAME])AS 'SQL_TABLE_VALUED_FUNCTION' FROM SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID
                                                              WHERE O.TYPE='TF'";

        public static readonly string GetAggregateFunctions = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ [NAME])AS 'SQL_AGGREGATE_FUNCTION' FROM SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID
                                                              WHERE O.TYPE='IF'";

        public static readonly string GetUserDefinedDataType = @"SELECT (SCHEMA_NAME(SCHEMA_ID) +'.'+[NAME])AS 'USERDEFINEDDATATYPE' FROM SYS.TYPES
                                                               WHERE IS_USER_DEFINED = 1";

        public static readonly string GetXmlSchemas = @"SELECT DISTINCT (SCHEMA_NAME(SCHEMA_ID)+'.'+XSC.NAME) AS 'NAME' FROM    SYS.XML_SCHEMA_COLLECTIONS XSC JOIN SYS.XML_SCHEMA_NAMESPACES XSN  ON (XSC.XML_COLLECTION_ID = XSN.XML_COLLECTION_ID)";

        public static readonly string[] GetServerProperties =
        {
            @"SELECT LEFT(@@VERSION, CHARINDEX(' - ', @@VERSION)) ProductName",
            @"SELECT SERVERPROPERTY('ProductMajorVersion') ProductMajorVersion",
            @"SELECT SERVERPROPERTY('ProductBuild') ProductBuild ",
            @"SELECT SERVERPROPERTY('InstanceDefaultLogPath') InstanceDefaultLogPath ",
            @"SELECT SERVERPROPERTY('Edition') Edition ",
            @"SELECT SERVERPROPERTY('BuildClrVersion') BuildClrVersion ",
            @"SELECT SERVERPROPERTY('Collation') COLLATION",
            @"SELECT SERVERPROPERTY('ComputerNamePhysicalNetBIOS') ComputerNamePhysicalNetBIOS",
            @"SELECT 
                CASE 
                    WHEN SERVERPROPERTY('EngineEdition')=1 THEN 'Personal or Desktop Engine (Not available in SQL Server 2005 (9.x) and later versions.)' 
                    WHEN SERVERPROPERTY('EngineEdition')=2 THEN 'Standard (This is returned for Standard, Web, and Business Intelligence.)' 
                    WHEN SERVERPROPERTY('EngineEdition')=3 THEN 'Enterprise (This is returned for Evaluation, Developer, and both Enterprise editions.)'
                    WHEN SERVERPROPERTY('EngineEdition')=4 THEN 'Express (This is returned for Express, Express with Tools and Express with Advanced Services)' 
                    WHEN SERVERPROPERTY('EngineEdition')=5 THEN 'SQL Database' WHEN SERVERPROPERTY('EngineEdition')=6 THEN 'SQL Data Warehouse' 
                    WHEN SERVERPROPERTY('EngineEdition')=8 THEN ' managed instance' END AS EngineEdition ",
            @"SELECT @@LANGUAGE AS LANGUAGE",
            @"SELECT (SELECT top 1 value_data FROM sys.dm_server_registry WHERE value_name='ObjectName')AS [Platform] ",
            @"SELECT 
                    CASE 
                        WHEN SERVERPROPERTY('IsClustered') =1 THEN 'Clustered' 
                        WHEN SERVERPROPERTY('IsClustered') =0 THEN 'Not Clustered' 
                   END AS IsClustered"
        };

        public static readonly string GetAdvancedServerSettings = @"select  value_name as Property , CAST(value_data as VARCHAR(1000))as   'Value'   from sys.dm_server_registry";
    }
}