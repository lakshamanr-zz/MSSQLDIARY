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
        public List<TableIndexInfo> LoadTableIndexes(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.LoadTableIndexes(astrTableName);
        }

        [HttpGet("[action]")]
        public TableCreateScript GetTableCreateScript(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableCreateScript(astrTableName);
        }

        [HttpGet("[action]")]
        public List<Tabledependencies> GetTableDependencies(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableDependencies(astrTableName);
        }

        [HttpGet("[action]")]
        public List<TableColumns> GetTableColumns(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableColumns(astrTableName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetTableDescription(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableDescription(astrTableName);
        }

        [HttpGet("[action]")]
        public List<TableFKDependency> GetTableForeignKeys(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableForeignKeys(astrTableName);
        }

        [HttpGet("[action]")]
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableKeyConstraints(astrTableName);
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
        public object GetDependencyTree(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
          return  JsonConvert.DeserializeObject(IsrvMssql.CreatorOrGetDependencyTree(astrTableName));
      
        }

        [HttpGet("[action]")]
        public List<TableFragmentationDetails> GetTableFragmentationDetails(string astrTableName, string astrDatabaseName)
        {
            IsrvMssql.IstrDatabaseConnection = GetActiveDatabaseInfo(astrDatabaseName);
            return IsrvMssql.GetTableFragmentationDetails(astrTableName);
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
            return new Ms_Description(); 
        }
        [HttpGet("[action]")]
        public Ms_Description GetErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string istrSchemaName, string selectedTables)
        { 
            return new Ms_Description(); 

        }
        [HttpGet("[action]")]
        public Ms_Description SaveErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string selectedTables, string astrSqlmodule)
        { 
            return new Ms_Description();  
        }
        [HttpGet("[action]")]
        public Ms_Description LoadErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string astrSqlmodule)
        {
            return new Ms_Description();  
        }
        [HttpGet("[action]")]
        public Ms_Description DeleteErDiagramWithSelectedTables(string astrDatabaseName, string istrServerName, string astrSqlmodule)
        { 
            return new Ms_Description(); 
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
        public FunctionCreateScript GetScalarFunctionCreateScript(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvScalarFunction.GetFunctionCreateScript(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetScalarFunctionsWithMsDescription(string astrDatabaseName)
        {
            return IsrvScalarFunction.GetFunctionsWithDescription(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetScalarFunctionDescription(string astrDatabaseName, string astrFunctionName)
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
        public List<PropertyInfo> GetTableValueFunctionsWithDescription(string astrDatabaseName)
        {
            return IsrvTableValueFunction.GetFunctionsWithDescription(astrDatabaseName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetTableValueFunctionWithDescription(string astrDatabaseName, string astrFunctionName)
        {
            return IsrvTableValueFunction.GetFunctionWithDescription(astrDatabaseName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateScalarFunctionDescription(string astrDatabaseName, string astrDescriptionValue, string astrFunctionName)
        {
            IsrvScalarFunction.CreateOrUpdateFunctionDescription(astrDatabaseName, astrDescriptionValue, astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableValueFunctionDescription(string astrDatabaseName, string astrDescriptionValue, string astrFunctionName)
        {
            IsrvTableValueFunction.CreateOrUpdateFunctionDescription(astrDatabaseName, astrDescriptionValue, astrFunctionName.Split(".")[0], astrFunctionName);
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
           return IsrvMssql.GetStoreProceduresWithDescription(astrDatabaseName); 
        } 
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
