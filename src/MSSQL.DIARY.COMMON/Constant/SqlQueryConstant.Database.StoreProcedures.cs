namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static readonly string GetStoreProceduresWithDescription = @"SELECT   ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])AS 'STOREPROC' , EP.VALUE AS [EXTENDED PROPERTY] FROM SYS.EXTENDED_PROPERTIES EP LEFT JOIN SYS.OBJECTS O ON EP.MAJOR_ID = O.OBJECT_ID   WHERE O.TYPE='P' AND   CLASS_DESC='OBJECT_OR_COLUMN'      UNION SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])AS 'STOREPROC'  ,'' AS [EXTENDED PROPERTY]     FROM SYS.OBJECTS O  WHERE O.TYPE='P'    ";

        public static readonly string GetStoreProcDependencies = @"SELECT OBJECT_SCHEMA_NAME ( referencing_id ) 	+ '.' +     OBJECT_NAME(referencing_id) AS referencing_object_name,     obj.type_desc AS referencing_object_type,     referenced_schema_name + '.' +     referenced_entity_name As referenced_object_name FROM sys.sql_expression_dependencies AS sed INNER JOIN sys.objects AS obj ON sed.referencing_id = obj.object_id WHERE referencing_id =OBJECT_ID(@StoreprocName)";

        public static readonly string GetStoreProceduresParametersWithDescriptions = @"select     'Parameter_name' = o.name,     'Type'   = type_name(user_type_id),     'Length'   = max_length,     'Prec'   = case when type_name(system_type_id) = 'uniqueidentifier'               then precision                else OdbcPrec(system_type_id, max_length, precision) end,     'Scale'   = OdbcScale(system_type_id, scale),     'Param_order'  = parameter_id,     'Collation'   = convert(sysname,                    case when system_type_id in (35, 99, 167, 175, 231, 239)                     then ServerProperty('collation') end)  , 				   ep.value  AS [Extendedproperty]  			      from sys.parameters o LEFT JOIN sys.extended_properties EP               				  ON 						ep.major_id = O.object_id and ep.minor_id=o.parameter_id where   object_id = object_id(@StoreprocName)";

        public static readonly string GetStoreProcedureCreateScript = @"SELECT  SCHEMA_NAME(schema_id)+'.'+[name] as StoreProc_name, object_definition(object_id) as [Proc Definition]FROM sys.objects WHERE type='P' and (SCHEMA_NAME(schema_id)+'.'+[name] )=@StoreprocName";

        public static readonly string GetExecutionPlan = @"SELECT  TOP 1  CAST( qp.query_plan as varchar(max)) as  query_plan,      CAST( CP.usecounts as VARCHAR(1000)) as usecounts,        CAST( cp.cacheobjtype AS VARCHAR(100)) AS CacheObjectType,        CAST( cp.size_in_bytes AS VARCHAR(01000)) AS Size_In_Byte,         CAST( SQLText.text as varchar(1000)) as SQLTEXT  FROM sys.dm_exec_cached_plans AS CP  CROSS APPLY sys.dm_exec_sql_text( plan_handle)AS SQLText  CROSS APPLY sys.dm_exec_query_plan( plan_handle)AS QP  WHERE objtype = 'Adhoc' and cp.cacheobjtype = 'Compiled Plan'";

        public static readonly string GetExecutionPlanOfStoreProc = @"SELECT   CAST([qp].[query_plan] as varchar(MAX)) as query_plan  , '' as usecounts,'' as CacheObjectType, '' as Size_In_Byte,'' as SQLTEXT  FROM       [sys].[dm_exec_procedure_stats] AS [ps]       JOIN [sys].[dm_exec_query_stats] AS [qs] ON [ps].[plan_handle] = [qs].[plan_handle]       CROSS APPLY [sys].[dm_exec_query_plan]([qs].[plan_handle]) AS [qp]WHERE     OBJECT_NAME([ps].[object_id], [ps].[database_id])  = @StoreprocName";

        public static readonly string GetStoreProcMsDescription = @"SELECT  ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])AS 'STOREPROC' , ep.value AS [Extended property]  FROM sys.extended_properties EP LEFT JOIN SYS.OBJECTS O ON ep.major_id = O.object_id  WHERE O.TYPE='P' and class_desc='OBJECT_OR_COLUMN' and ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])=@StoreprocName";

        public static readonly string UpdateStoreProcExtendedProperty = @"EXEC 
                sys.sp_updateextendedproperty 
                N'MS_Description',  
                @sp_value , 
                N'SCHEMA', 
                @Schema_Name, 
                N'PROCEDURE', 
                @sp_Name";

        public static readonly string InsertStoreProcExtendedProperty = @"EXEC
                sys.sp_addextendedproperty
                N'MS_Description',
                @sp_value ,
                N'SCHEMA',
                @Schema_Name,
                N'PROCEDURE',
                @sp_Name";


        public static readonly string UpdateStoreProcParameterExtendedProperty = @"EXEC
                sys.sp_updateextendedproperty
                N'MS_Description',
                @sp_value ,
                N'SCHEMA',
                @Schema_Name,
                N'PROCEDURE',
                @sp_Name, 
                N'PARAMETER', 
                @parmeterName";

        public static readonly string InsertStoreProcParameterExtendedProperty = @"EXEC
                sys.sp_addextendedproperty
                N'MS_Description',
                @sp_value ,
                N'SCHEMA',
                @Schema_Name,
                N'PROCEDURE',
                @sp_Name, 
                N'PARAMETER', 
                @parmeterName";
    }
}