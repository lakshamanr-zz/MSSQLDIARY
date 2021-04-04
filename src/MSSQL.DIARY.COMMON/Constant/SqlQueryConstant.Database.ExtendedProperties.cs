namespace MSSQL.DIARY.COMN.Constant
{
    public static partial class SqlQueryConstant
    {
        public static string AddSpExtendedproperty = @"EXECUTE sys.sp_addextendedproperty MS_Description,@Properydesc,schema,@schemaName,table,@tableName,column,@columnName";

        /* and then update the description of the  dbo.Customer.InsertionDate column  */
        public static string UpdateSpExtendedproperty = @" EXECUTE sys.sp_updateextendedproperty MS_Description,@Properydesc,schema,@schemaName,table,@tableName,column,@columnName";

        /* we can list this column */
        public static string SelectFnListextendedproperty = @" SELECT *  FROM::fn_listextendedproperty (MS_Description,@Properydesc,schema,@schemaName,table,@tableName,column,@columnName)";

        public static string DeleteSpDropextendedproperty = @"EXECUTE sys.sp_dropextendedproperty  MS_Description,@Properydesc,schema,@schemaName,table,@tableName,column,@columnName";

        public static string GetListOfExtendedPropertiesList = @" SELECT     distinct sep.Name  FROM   sys.tables t left JOIN     sys.extended_properties sep ON t.object_id = sep.major_id where sep.Name  is not null";
    }
}