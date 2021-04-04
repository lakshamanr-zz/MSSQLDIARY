namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static string GetFunctionsWithDescription = @"SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.[NAME])AS 'SQL_TABLE_VALUED_FUNCTION' ,sep.value FROM  SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O  ON M.OBJECT_ID=O.OBJECT_ID INNER JOIN sys.extended_properties sep ON o.object_id=sep.major_id  WHERE O.TYPE=@function_Type  and     sep.Name = 'MS_Description'    AND sep.minor_id = 0  ";

        public static string GetFunctionProperties = @"SELECT CAST(uses_ansi_nulls as varchar(100)) uses_ansi_nulls , CAST(uses_quoted_identifier as varchar(100)) as uses_quoted_identifier,CAST(create_date as varchar(100)) create_date,CAST(modify_date as varchar(100)) modify_date,CAST( o.name as varchar(100)) [name] FROM  SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O  ON M.OBJECT_ID=O.OBJECT_ID WHERE O.TYPE=@function_Type    and o.name=@function_name";

        public static string GetFunctionParameters = @" SELECT   'Parameter_name' = o.name,     'Type'   = type_name(user_type_id),     'Length'   = max_length,     'Prec'   = case when type_name(system_type_id) = 'uniqueidentifier'               then precision                else OdbcPrec(system_type_id, max_length, precision) end,     'Scale'   = OdbcScale(system_type_id, scale),     'Param_order'  = parameter_id,     'Collation'   = convert(sysname,                    case when system_type_id in (35, 99, 167, 175, 231, 239)                     then ServerProperty('collation') end)  , 		 		   sep.value  AS [Extendedproperty]    FROM   SYS.SQL_MODULES M  INNER JOIN SYS.OBJECTS Obj  ON M.OBJECT_ID=Obj.OBJECT_ID   INNER JOIN sys.extended_properties sep ON Obj.object_id=sep.major_id    INNER JOIN sys.parameters o ON sep.major_id = O.object_id        WHERE Obj.TYPE=@function_Type  and     sep.Name = 'MS_Description'    AND sep.minor_id = 0 and O.object_id = object_id(@function_name) ";

        public static string GetFunctionCreateScript = @"SELECT [definition] FROM   SYS.SQL_MODULES M  INNER JOIN SYS.OBJECTS Obj  ON M.OBJECT_ID=Obj.OBJECT_ID   INNER JOIN sys.extended_properties sep ON Obj.object_id=sep.major_id    INNER JOIN sys.parameters o ON sep.major_id = O.object_id        WHERE Obj.TYPE=@function_Type  and     sep.Name = 'MS_Description'    AND sep.minor_id = 0 and O.object_id = object_id(@function_name) ";

        public static string GetFunctionDependencies = @"  declare @Table table ( [name] varchar(100), [type] varchar(1000), Updated varchar(1000), selected varchar(1000), column_name varchar(1000) )INSERT INTO @Table exec sp_depends @function_name select * from @Table ";

        public static readonly string UpdateFunctionExtendedProperty = @"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@fun_value,@level0type=N'SCHEMA',@level0name=@Schema_Name,@level1type=N'FUNCTION',@level1name=@FunctionName";

        public static readonly string CreateFunctionExtendedProperty = @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@fun_value,@level0type=N'SCHEMA',@level0name=@Schema_Name,@level1type=N'FUNCTION',@level1name=@FunctionName";

    }
}