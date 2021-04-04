namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static string GetUserDefinedDataTypes = @"select SCHEMA_NAME(SCHEMA_ID)+'.'+name as Name ,is_nullable AS 'Allow Nulls', TYPE_NAME(system_type_id) as 'Base Type Name', max_length as 'Length' , CAST('CREATE TYPE '+SCHEMA_NAME(SCHEMA_ID)+'.'+name +' FROM  '+TYPE_NAME(system_type_id)+' ( '+CAST(+max_length AS nvarchar)+ ' ) ' +  CASE WHEN  is_nullable=1 THEN 'NULL' ELSE 'NOT NULL ' END  as nvarchar(100))as createscript  from  sys.types t    where is_user_defined = 1 ";
        public static string GetUserDefinedDataTypeDetails = @"select SCHEMA_NAME(SCHEMA_ID)+'.'+name as Name,is_nullable AS 'Allow Nulls', TYPE_NAME(system_type_id) as 'Base Type Name', max_length as 'Length' , CAST('CREATE TYPE '+SCHEMA_NAME(SCHEMA_ID)+'.'+name +' FROM  '+TYPE_NAME(system_type_id)+' ( '+CAST(+max_length AS nvarchar)+ ' ) ' +  CASE WHEN  is_nullable=1 THEN 'NULL' ELSE 'NOT NULL ' END  as nvarchar(100))as createscript  from  sys.types t    where is_user_defined = 1 and    SCHEMA_NAME(SCHEMA_ID)+'.'+name =@TypeName";
        public static string GetUsedDefinedDataTypeReference = @"select s.name +'.'+o.name as ObjectName ,o.type   from sys.schemas s join sys.objects o    on o.schema_id = s.schema_id  join sys.columns c    on c.object_id = o.object_id  join sys.types t    on c.user_type_id = t.user_type_id and is_user_defined = 1   and SCHEMA_NAME(t.schema_id) +'.'+t.name=@TypeName";
        public static string GetUsedDefinedDataTypeExtendedProperties = @"SELECT value  FROM::fn_listextendedproperty ('MS_Description','SCHEMA',@SchemaName ,N'TYPE',@TypeName,null,null)";
        public static string AddUserDefinedDataTypeExtendedProperty = @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@desc , @level0type=N'SCHEMA',@level0name=@SchemaName, @level1type=N'TYPE',@level1name=@TypeName";
        public static string UpdateUserDefinedDataTypeExtendedProperty = @"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@desc , @level0type=N'SCHEMA',@level0name=@SchemaName, @level1type=N'TYPE',@level1name=@TypeName";


    }
}
