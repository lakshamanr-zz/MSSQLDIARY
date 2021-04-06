using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]

    // ReSharper disable once InconsistentNaming
    public class MSSQLController : BaseController
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<MSSQLController> _logger;
        public SrvMssql IsrvTableValueFunction { get; set; }
        public SrvMssql IsrvScalarFunction { get; set; }
        private SrvMssql IsrvMssql { get; }

        public MSSQLController(ILogger<MSSQLController> logger ,ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            _logger = logger;

            IsrvTableValueFunction = new SrvMssql("TF");
            IsrvScalarFunction = new SrvMssql("FN");
            IsrvMssql = new SrvMssql();
        }

        #region DatabaseTables

        [HttpGet("[action]")]
        public List<TablePropertyInfo> GetTablesDescription(string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTablesDescription();
        }

        [HttpGet("[action]")]
        public List<TableIndexInfo> LoadTableIndexes(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.LoadTableIndexes(astrtableName);
        }

        [HttpGet("[action]")]
        public TableCreateScript GetTableCreateScript(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableCreateScript(astrtableName);
        }

        [HttpGet("[action]")]
        public List<Tabledependencies> GetTableDependencies(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableDependencies(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableColumns> GetTableColumns(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableColumns(astrtableName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetTableDescription(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableDescription(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableFKDependency> GetTableForeignKeys(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableForeignKeys(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableKeyConstraints(astrtableName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateColumnDescription(string astrTableName, string astrDatabaseName, string astrDescriptionValue,
            string astrColumnName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.CreateOrUpdateColumnDescription(astrDescriptionValue,
                astrTableName.Split(".")[0], astrTableName, astrColumnName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableDescription(string astrTableName, string astrDatabaseName, string astrDescriptionValue)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            IsrvMssql.CreateOrUpdateTableDescription(astrDescriptionValue, astrTableName.Split(".")[0], astrTableName);
            return true;
        }

        [HttpGet("[action]")]
        public object GetDependencyTree(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            var returnResult = JsonConvert.DeserializeObject(IsrvMssql.CreatorOrGetDependencyTree(astrtableName));
            return returnResult;
        }

        [HttpGet("[action]")]
        public List<TableFragmentationDetails> GetTableFragmentationDetails(string astrtableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableFragmentationDetails(astrtableName);
        }

        #endregion 

        #region Databasse

        [HttpGet("[action]")]
        public string GetDatabaseUserDefinedText()
        {
            return "";
        }

        [HttpGet("[action]")]
        public List<string> GetDatabaseObjectTypes()
        {
            return IsrvMssql.GetDatabaseObjectTypes();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetDatabaseProperties(string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetDatabaseProperties();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetDatabaseOptions(string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetDatabaseOptions();
        }

        [HttpGet("[action]")]
        public List<FileInfomration> GetDatabaseFiles(string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetDatabaseFiles();
        }

        #endregion

        #region DatabaseER-Diagram

        [HttpGet("[action]")]
        public Ms_Description GetErDiagram(string astrDatabaseName, string istrServerName, string istrSchemaName)
        {
            var result = new Ms_Description();
            //if (istrSchemaName.Equals("All"))
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName, istrServerName, null)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName.Split('/')[0],
            //            astrDatabaseName.Split('/')[1], null);
            //else
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName, istrServerName, istrSchemaName)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName.Split('/')[0],
            //            astrDatabaseName.Split('/')[1], istrSchemaName);


            return result;
        }
        [HttpGet("[action]")]
        public Ms_Description GetErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string istrSchemaName, string selectedTables)
        {
            //var alstOfSelectedTables = selectedTables.Split(';').Where(x => x.IsNotNullOrEmpty()).ToList();
            //var newSelectedTables = new List<string>();
            //alstOfSelectedTables.ForEach(x => {
            //    newSelectedTables.Add(x.Split('.')[1]);
            //});
            var result = new Ms_Description();
            //if (istrSchemaName.Equals("All"))
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName, istrServerName, null, newSelectedTables)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName.Split('/')[0],
            //            astrDatabaseName.Split('/')[1], null, newSelectedTables);
            //else
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName, istrServerName, istrSchemaName, newSelectedTables)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName.Split('/')[0],
            //            astrDatabaseName.Split('/')[1], istrSchemaName, newSelectedTables);


            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description SaveErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string selectedTables, string istrsqlmodule)
        {

            var result = new Ms_Description();

            //DatabaseModule databaseModule = new DatabaseModule();
            //databaseModule.DatabaseName = astrDatabaseName;
            //databaseModule.ServerName = istrServerName;
            //databaseModule.tables = SelectedTables;
            //databaseModule.DbModuleName = istrsqlmodule;
            //if (!applicationDbContext.databaseModule
            //    .Where(x => x.DatabaseName.Contains(astrDatabaseName) && x.ServerName.Contains(istrServerName) && x.DbModuleName.Contains(istrsqlmodule))
            //    .Any())
            //{
            //    applicationDbContext.databaseModule.Add(databaseModule);
            //    applicationDbContext.SaveChanges();
            //    result.desciption = "Module save successfully ";
            //}
            //else
            //{
            //    result.desciption = "There is already same name module is existing in database";
            //}
            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description LoadErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string istrsqlmodule)
        {

            var result = new Ms_Description();
            //var sqlmodule =applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(astrDatabaseName) &&
            //     x.ServerName.Contains(istrServerName) &&
            //     x.DbModuleName.Contains(istrsqlmodule)

            //     ).FirstOrDefault() ;
            // if (sqlmodule.IsNotNull())
            // {
            //     var alstOfSelectedTables = sqlmodule.tables.Split(';').Where(x => x.IsNotNullOrEmpty()).ToList();
            //     var newSelectedTables = new List<string>();
            //     alstOfSelectedTables.ForEach(x =>
            //     {
            //         newSelectedTables.Add(x.Split('.')[1]);
            //     });
            //     result.desciption = !istrServerName.IsNullOrEmpty()
            //         ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName, istrServerName, null, newSelectedTables)
            //         : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, astrDatabaseName.Split('/')[0],
            //             astrDatabaseName.Split('/')[1], null, newSelectedTables);
            // } 

            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description DeleteErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string istrsqlmodule)
        {

            var result = new Ms_Description();
            //var sqlmodule = applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(astrDatabaseName) &&
            //     x.ServerName.Contains(istrServerName) &&
            //     x.DbModuleName.Contains(istrsqlmodule)

            //     ).FirstOrDefault();
            //if (sqlmodule.IsNotNull())
            //{
            //    applicationDbContext.databaseModule.Remove(sqlmodule);
            //    applicationDbContext.SaveChanges();
            //}

            return result;

        }
        [HttpGet("[action]")]
        public List<string> LoadAllModules(string astrDatabaseName, string istrServerName)
        {

            var result = new List<string>();
            //var sqlmodule = applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(astrDatabaseName) &&
            //     x.ServerName.Contains(istrServerName));
            //if (sqlmodule.IsNotNull())
            //{
            //    result = sqlmodule.Select(x => x.DbModuleName).ToList();
            //}

            return result;

        }

        #endregion

        #region Object Explorer

        [HttpGet]
        public string GetObjectExplorerDetails(string astrDatabaseServerName, string astrDatabaseName)
        {
            var lstrActiveDatabase = GetActiveDatabaseInfo();

            if (lstrActiveDatabase.IsNotNullOrEmpty())
            {

                return SrvMssql.ObjectExplorerDetails.GetOrCreate(lstrActiveDatabase, GetObjectExplorer);

            }

            if (SrvMssql.ObjectExplorerDetails.Cache.Count > 0)
            {

                return SrvMssql.ObjectExplorerDetails.GetOrCreate(lstrActiveDatabase, GetObjectExplorer);
            }
            return string.Empty;

        }
        private string GetObjectExplorer()
        {
            return @"{""data"":" + JsonConvert.SerializeObject(SrvMssql.GetObjectExplorer(GetActiveDatabaseInfo(), GetActiveDatabaseName())) + "}";
        }

        #endregion

        #region Database Functions

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetScalerFunctionDependencies(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionDependencies(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetScalerFunctionProperties(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionProperties(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetScalerFunctionParameters(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionParameters(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetScalerFunctionCreateScript(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionCreateScript(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllScalarFunctionWithMsDescriptions(string astrDatabaseName)
        {
            return IsrvScalarFunction.GetFunctionsWithDescription(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetScalarFunctionMsDescriptions(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionWithDescription(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetTableValueFunctionDependencies(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionDependencies(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetTableValueFunctionProperties(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionProperties(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetTableValueFunctionParameters(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionParameters(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetTableValueFunctionCreateScript(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionCreateScript(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllTableValueFunctionWithMsDescriptions(string astrDatabaseName)
        {
            return IsrvTableValueFunction.GetFunctionsWithDescription(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetTableValueFunctionMsDescriptions(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionWithDescription(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateScalerFunctionDescription(string astrDatabaseName, string astrDescriptionValue, string astrFunctionName)
        {
            IsrvScalarFunction.CreateOrUpdateFunctionDescription(astrDatabaseName, astrDescriptionValue,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableValueFunctionDescription(string astrDatabaseName, string astrDescriptionValue, string astrFunctionName)
        {
            IsrvTableValueFunction.CreateOrUpdateFunctionDescription(astrDatabaseName, astrDescriptionValue,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }

        #endregion

        #region Database schema


        [HttpGet("[action]")]
        public List<PropertyInfo> GetSchemaWithDescriptions(string astrDatabaseName)
        {
            return IsrvMssql.GetSchemaWithDescriptions(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateSchemaMsDescription(string astrDatabaseConnection, string astrDescriptionValue, string astrSchemaName)
        {
              IsrvMssql.CreateOrUpdateSchemaMsDescription(astrDatabaseConnection, astrDescriptionValue, astrSchemaName);
        }

        [HttpGet("[action]")]
        public List<SchemaReferanceInfo> GetSchemaReferences(string astrDatabaseName, string astrSchemaName)
        {
            return IsrvMssql.GetSchemaReferences(astrDatabaseName, astrSchemaName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetSchemaDescription(string astrDatabaseName, string astrSchemaName)
        {
            return IsrvMssql.GetSchemaDescription(astrDatabaseName, astrSchemaName);
        }

        #endregion

        #region Database Server


        [HttpGet("[action]")]
        public string GetServerInformation()
        {
            return GetActiveServerName();
        }

        [HttpGet("[action]")]
        public List<DatabaseName> GetDatabaseNames()
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo();
            return IsrvMssql.GetDatabaseNames();
        }
        [HttpGet("[action]")]
        public List<DatabaseName> GetDatabaseNames(string astrServerName)
        {
            IsrvMssql.IstrDatabaseConnection = GetConnectionString(astrServerName);
            List<DatabaseName> lstDatabaseName = new List<DatabaseName>
            {
                new DatabaseName {databaseName = "Select Database"}
            };
            lstDatabaseName.AddRange(IsrvMssql.GetDatabaseNames());
            return lstDatabaseName;
        }


        [HttpGet("[action]")]
        public List<PropertyInfo> GetServerProperties()
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo();
            return IsrvMssql.GetServerProperties();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo();
            return IsrvMssql.GetAdvancedServerSettings();
        }

        [HttpGet("[action]")]
        public List<string> GetServerNameList()
        {
            return LoadServerList();
        }
        [HttpGet("[action]")]
        public string SetDefaultDatabase(string astrServerName, string astrDatabaseName)
        {
            SetDefaultDatabaseActive(astrServerName, astrDatabaseName);
            return string.Empty;
        }

        [HttpGet("[action]")]
        public string GetDefaultDatabase()
        {
            try
            {
                return GetActiveServerName() + ";" + GetActiveDatabaseName();
            }
            catch (Exception)
            {
                // ignored
            }

            return string.Empty;
        }

        #endregion

        #region Store Procedure


        [HttpGet("[action]")]
        public List<SP_PropertyInfo> GetStoreProceduresWithDescription(string astrDatabaseName, bool ablnSearchInSsisPackages)
        {
            var lstStoreprocedure = IsrvMssql.GetStoreProceduresWithDescription(astrDatabaseName);
        
            //var SSRS_package = new List<PackageJsonHandler>();
            //SSISPackageInfoHandlerController.GetAllSSISPackages(_hostingEnv.WebRootPath);
            //SSISPackageInfoHandlerController.SSISPkgeCache.Cache.TryGetValue(serverName, out SSRS_package);
            //if (SSRS_package != null) FillSSISPackageDetails(AllStoreprocedure, SSRS_package);
            //if (ablnSearchInSsisPackages)
            //    lstStoreprocedure = lstStoreprocedure.Where(x => x.lstSSISpackageReferance.IsNotNull()).ToList();
            return lstStoreprocedure;
        }

        //private static void FillSSISPackageDetails(List<SP_PropertyInfo> AllStoreprocedure,
        //    List<PackageJsonHandler> SSRS_package)
        //{
        //    AllStoreprocedure.ForEach(x =>
        //    {
        //        SSRS_package.ForEach(x1 =>
        //        {
        //            x1.ExecuteSQLTask.ForEach(x3 =>
        //            {
        //                if (x3.SqlStatementSource.Contains(x.istrName))
        //                {
        //                    if (x.lstSSISpackageReferance == null) x.lstSSISpackageReferance = new List<string>();

        //                    x.lstSSISpackageReferance.Add(x1.PackageLocation);
        //                }
        //            });
        //        });
        //        if (x.lstSSISpackageReferance != null)
        //            x.lstSSISpackageReferance = x.lstSSISpackageReferance.DistinctBy(x1 => x1).ToList();
        //    });
        //}

        [HttpGet("[action]")]
        public Ms_Description GetStoreProcedureCreateScript(string astrDatabaseName, string astrStoreProcedureName)
        {
            return IsrvMssql.GetStoreProcedureCreateScript(astrDatabaseName, astrStoreProcedureName);
        }

        [HttpGet("[action]")]
        public List<SP_Depencancy> GetStoreProceduresDependency(string astrDatabaseName, string astrStoreProcedureName)
        {
            return IsrvMssql.GetStoreProceduresDependency(astrDatabaseName, astrStoreProcedureName);
        }

        [HttpGet("[action]")]
        public List<SP_Parameters> GetStoreProceduresParametersWithDescription(string astrDatabaseName, string astrStoreProcedureName)
        {
            return IsrvMssql.GetStoreProceduresParametersWithDescription(astrDatabaseName, astrStoreProcedureName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetStoreProcedureDescription(string astrDatabaseName, string astrStoreProcedureName)
        {
            return IsrvMssql.GetStoreProcedureDescription(astrDatabaseName, astrStoreProcedureName);
        }

        [HttpGet("[action]")]
        public List<ExecutionPlanInfo> GetStoreProcedureExecutionPlan(string astrDatabaseName, string astrStoreProcedureName)
        {
            return IsrvMssql.GetStoreProcedureExecutionPlan(astrDatabaseName, astrStoreProcedureName);
        }

        //[HttpGet("[action]")]
        //public object GetDependancyTree(string astrDatabaseName, string astrStoreProcedureName)
        //{
        //    return JsonConvert.DeserializeObject(
        //        ISrvMssql.CreatorOrGetStoreProcedureDependencyTree(astrDatabaseName, astrStoreProcedureName));
        //}

        [HttpGet("[action]")]
        public void CreateOrUpdateStoreProcParameterDescription(string astrDatabaseName, string astrDescriptionValue, string astrSpName, string astrSpParameterName)
        {
            IsrvMssql.CreateOrUpdateStoreProcParameterDescription(astrDatabaseName, astrDescriptionValue, astrSpName.Split(".")[0], astrSpName, astrSpParameterName);
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateStoreProcDescription(string astrDatabaseName, string astrDescriptionValue, string astrSpName)
        {
            IsrvMssql.CreateOrUpdateStoreProcedureDescription(astrDatabaseName, astrDescriptionValue, astrSpName.Split(".")[0], astrSpName);
        }

        #endregion

        #region Database Triggers

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllDatabaseTrigger(string astrDatabaseName)
        {
            return IsrvMssql.GetTriggers(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public TriggerInfo GetTriggerInfosByName(string astrDatabaseName, string istrTriggerName)
        {
            return IsrvMssql.GetTrigger(astrDatabaseName, istrTriggerName).FirstOrDefault();
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateTriggerDescription(string astrDatabaseName, string astrDescriptionValue, string astrTriggerName)
        {
            IsrvMssql.CreateOrUpdateTriggerDescription(astrDatabaseName, astrDescriptionValue, astrTriggerName);
        }


        #endregion

        #region UserDefinedFuncations

        [HttpGet("[action]")]
        public List<UserDefinedDataTypeDetails> GetAllUserDefinedDataTypes(string astrDatabaseName)
        {
            return IsrvMssql.GetUserDefinedDataTypes(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public UserDefinedDataTypeDetails GetUserDefinedDataTypeDetails(string astrDatabaseName, string astrTypeName)
        {
            return IsrvMssql.GetUserDefinedDataType(astrDatabaseName, astrTypeName);
        }

        [HttpGet("[action]")]
        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReferance(string astrDatabaseName,
            string astrTypeName)
        {
            return IsrvMssql.GetUsedDefinedDataTypeReference(astrDatabaseName, astrTypeName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string astrTypeName)
        {
            return IsrvMssql.GetUsedDefinedDataTypeExtendedProperties(astrDatabaseName, astrTypeName);
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string astrTypeName, string astrDescValue)
        {
            IsrvMssql.CreateOrUpdateUsedDefinedDataTypeExtendedProperties(astrDatabaseName, astrTypeName, astrDescValue);
        }

        #endregion

        #region Database Views
        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllViewsDetails(string astrDatabaseName)
        {
            string lstrDbConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetViewsWithDescription(lstrDbConnection);
        }

        [HttpGet("[action]")]
        public List<ViewDependancy> GetViewDependencies(string astrDatabaseName, string astrViewName)
        {
            return IsrvMssql.GetViewDependencies(astrDatabaseName, astrViewName);
        }

        [HttpGet("[action]")]
        public List<View_Properties> GetViewProperties(string astrDatabaseName, string astrViewName)
        {
            return IsrvMssql.GetViewProperties(astrDatabaseName, astrViewName);
        }

        [HttpGet("[action]")]
        public List<ViewColumns> GetViewColumns(string astrDatabaseName, string astrViewName)
        {
            return IsrvMssql.GetViewColumns(astrDatabaseName, astrViewName);
        }

        [HttpGet("[action]")]
        public ViewCreateScript GetViewCreateScript(string astrDatabaseName, string astrViewName)
        {
            return IsrvMssql.GetViewCreateScript(astrDatabaseName, astrViewName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetViewsWithDescription(string astrDatabaseName, string astrViewName)
        {
            return IsrvMssql.GetViewsWithDescription(astrDatabaseName, astrViewName);
        }


        #endregion

        #region File upload

        [HttpPost, DisableRequestSizeLimit]
        [Route("api/[controller]")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var serverName = GetActiveServerName();
                var databaseName = GetActiveDatabaseName();
                var folderName = Path.Combine("Resources", "Excel");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    pathToSave += "\\" + serverName + "\\" + databaseName;
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }
                    var fullPath = Path.Combine(pathToSave, fileName);

                    if (fileName.Contains("xlsx"))
                    {
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        return Ok(new { dbPath });
                    }
                    else
                    {
                        return BadRequest();
                    }


                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #endregion
    }
}
