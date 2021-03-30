using System.Collections.Generic;

namespace MSSQL.DIARY.SRV.Interfaces
{
    public interface ISrvTreeView
    {
        string istrRootPath { get; set; }

        void CreateDatabasaeFolder(string istrlocation);
        void CreateDatabasaeProgrammabilityFolder(string istrUserDatabaseFolder);
        void CreateDatabasaeTableFolder(string istrlocation);
        void CreateDatabasaeTablesFolder(string istrlocation);
        void CreateDatabasaeViewsFolder(string istrUserDatabaseFolder);
        void CreateDatabaseaggregatedFunctionfolder(string istrUserDatabaseProgrammabilityTableValueFunctionFolder);
        void CreateDatabaseaggregatedFunctionsFolder(string istrUserDatabaseProgrammabilityTableValueFunctionFolder);
        void CreateDatabaseDatabaseTriggerFolder(string istrUserDatabaseProgrammabilityTriggerFolder);
        void CreateDatabaseDatabaseTriggersFolder(string istrUserDatabaseProgrammabilityTriggersFolder);
        void CreateDatabaseFunctionsFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseScalarValueFunctionFolders(string istrUserDatabaseProgrammabilityScalarFunctionFolder);
        void CreateDatabaseScalarValueFunctionsFolder(string istrUserDatabaseProgrammabilityScalarFunctionFolder);
        void CreateDatabaseStoredProcedureFolder(string istrUserDatabaseStoredProceduresFolder);
        void CreateDatabaseStoredProceduresFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseTableValueFunctionFolder(string istrUserDatabaseProgrammabilityTableValueFunctionFolder);
        void CreateDatabaseTableValueFunctionsFolder(string istrUserDatabaseProgrammabilityTableValueFunctionFolder);
        void CreateDatabaseTypeFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseUserDefinedDataTypeFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseUserDefinedDataTypesFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseXmlSchemaCollectionFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDatabaseXmlSchemaCollectionsFolder(string istrUserDatabaseProgrammabilityFolder);
        void CreateDirtStructForJsonFiles(string astrRootPath = null);
        void CreateServerNameFolders(string istrlocation);
        void CreateTreeViewJsonFolder(string istrlocation);
        void CreateUserDatabasaeFolder(string istrlocation);
        List<string> GetAggregateFunctions();
        List<string> GetDatabaseName();
        List<string> GetDatabaseRoles();
        List<string> GetFulllTextLogs();
        List<string> GetScalarFunctions();
        List<string> GetSchemas();
        List<string> GetServerName();
        List<string> GetStoreProcedures();
        List<string> GetTables();
        List<string> GetTableValueFunctions();
        List<string> GetTriggers();
        List<string> GetUserDefinedTypes();
        List<string> GetViews();
        List<string> GetXmlSchemaCollection();
    }
}