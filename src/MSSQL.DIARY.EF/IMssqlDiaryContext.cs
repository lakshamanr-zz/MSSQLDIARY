using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public interface IMssqlDiaryContext
    {
        string GetDatabaseName { get; }
        string IstrDbConnections { get; set; }

        void CreateOrUpdateColumnDescription(string astrDescription_Value, string astrSchema_Name, string astrTableName,
            string astrColumnValue);

        void CreateOrUpdateStoreProcDescription(string astrDescription_Value, string astrSchema_Name,
            string StoreprocName, string ParameterName = null);

        void CreateOrUpdateTableDescription(string astrDescription_Value, string astrSchema_Name, string astrTableName);
        List<PropertyInfo> GetAdvancedServerSettingsInfo();
        List<string> GetAggregateFunctions();
        List<PropertyInfo> GetAllFunctionWithMsDescriptions(string function_type);
        List<TableKeyConstraint> GetTableKeyConstraints(string istrtableName);
        List<PropertyInfo> GetAllStoreprocedureDescription();
        List<Tabledependencies> GetAllTableDependencies(string istrtableName);
        List<TablePropertyInfo> GetAllTableDescription();
        List<TableFKDependency> GetAllTableForeignKeys(string istrtableName);
        List<TableColumns> GetAllTablesColumn(string istrtableName);
        List<PropertyInfo> GetAllViewsDetailsWithms_description();
        List<ExecutionPlanInfo> GetCachedExecutionPlan(string StoreprocName);
        Ms_Description GetCreateScriptOfStoreProc(string StoreprocName);
        List<string> GetDatabaseNames();
        string GetDatabaseServerName();
        List<FileInfomration> GetdbFiles();
        List<PropertyInfo> GetdbOptions();
        List<PropertyInfo> GetdbProperties();
        FunctionCreateScript GetFunctionCreateScript(string astrFunctionName, string function_type);
        List<FunctionDependencies> GetFunctionDependencies(string astrFunctionName, string function_type);
        List<FunctionParameters> GetFunctionParameters(string astrFunctionName, string function_type);
        List<FunctionProperties> GetFunctionProperties(string astrFunctionName, string function_type);
        List<ReferencesModel> GetObjectOnWhichDepends(string astrObjectName);
        List<ReferencesModel> GetObjectThatDependsOn(string astrObjectName);
        List<string> GetScalarFunction();
        List<PropertyInfo> GetServerProperties();
        List<SP_Depencancy> GetStoreProcDependancy(string StoreprocName);
        List<string> GetStoreProcedures();
        string GetStoreProcMsDescription(string StoreprocName);
        List<SP_Parameters> GetStoreProcParameters(string StoreprocName);
        List<string> GetTableColumns(string istrTableName);
        TableCreateScript GetTableCreateScript(string istrtableName);
        Ms_Description GetTableDescription(string istrtableName);
        List<TableIndexInfo> LoadTableIndexes(string istrtableName);
        List<string> GetTables();
        List<string> GetTableValueFunction();
        List<string> GetTigger();
        List<string> GetUserDefinedDataType();
        List<ViewColumns> GetViewColumns(string astrViewName);
        ViewCreateScript GetViewCreateScript(string astrViewName);
        List<View_Properties> GetViewProperties(string astrViewName);
        List<string> GetViews();
        List<ViewDependancy> GetViewDependancies(string astrViewName);
        List<string> GetWorkDetailsbyName(string astrWorkFlowName);
        List<string> GetWorkList();
        List<string> GetXmlSchemaCollection();
        bool IsLoginSuccessfully(ServerLogin serverLogin);
    }
}