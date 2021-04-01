namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static readonly string GetTriggers = @"select  trigg.NAME AS  'Tiggers' ,sep.value from sys.triggers trigg inner join sys.sql_modules module ON trigg.object_id=module.object_id left join sys.extended_properties sep ON sep.major_id=trigg.object_id where trigg.parent_id=0 union SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.NAME)AS 'Tiggers' ,sep.value FROM SYS.SQL_MODULES M left JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID left JOIN sys.extended_properties sep ON o.object_id=sep.major_id WHERE O.TYPE='TR'";
        public static readonly string GetTrigger = @"select * from (SELECT ((SCHEMA_NAME(O.SCHEMA_ID) )+'.'+ O.NAME)AS 'Tiggers' , sep.value ,m.definition,o.create_date,o.modify_date FROM  SYS.SQL_MODULES M INNER JOIN SYS.OBJECTS O ON M.OBJECT_ID=O.OBJECT_ID INNER JOIN sys.extended_properties sep ON o.object_id=sep.major_id WHERE O.TYPE='TR' UNION select   trigg.NAME AS 'Tiggers',   sep.value ,      module.definition,         ISNULL(ob.create_date,''),         ISNULL(ob.modify_date,'') from   sys.triggers trigg    inner join      sys.sql_modules module       ON trigg.object_id = module.object_id    left join      sys.extended_properties sep       ON sep.major_id = trigg.object_id   left join sys.objects ob on trigg.object_id=ob.object_id where   trigg.parent_id = 0 )  as Temp where Tiggers=@TiggersName";
        public static readonly string UpdateTriggerExtendedProperty = @"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@Trigger_value,@level0type=N'TRIGGER',@level0name=@Trigger_Name";
        public static readonly string CreateTriggerExtendedProperty = @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@Trigger_value,@level0type=N'TRIGGER',@level0name=@Trigger_Name";

    }
}
