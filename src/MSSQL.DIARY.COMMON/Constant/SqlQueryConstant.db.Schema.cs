namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static string GetSchemaWithDescriptions = @" SELECT sch.name ,''FROM sys.schemas sch where schema_id<=100 ";
        public static string GetSchemaInfoByName = @"";
        public static string GetSchemaReferences = @"select name as table_name from sys.tables where schema_name(schema_id) = @schema_id order by name;";
        public static string GetSchemaMsDescription = @" SELECT value,sch.name FROM sys.extended_properties ep left JOIN sys.schemas sch ON class=3 AND ep.major_id=SCHEMA_ID  where ep.class=3 and sch.name =@schemaName";
        public static string UpdateSchemaColumnExtendedProperty = @"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@Schema_info ,@level0type=N'SCHEMA',@level0name=@SchemaName";
        public static string CreateSchemaColumnExtendedProperty = @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@Schema_info ,@level0type=N'SCHEMA',@level0name=@SchemaName";
    }
}
