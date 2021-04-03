using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DatabaseTablesController : ApplicationBaseController
    {

        private readonly ApplicationDbContext applicationDbContext;


        public DatabaseTablesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            ISrvMssql = new SrvMssql(); 
            this.applicationDbContext = context;
        }
        private SrvMssql ISrvMssql { get; }

        [HttpGet("[action]")]
        public List<TablePropertyInfo> GetAllDatabaseTable(string astrdbName)
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName); 
           return ISrvMssql.GetTablesDescription(); 
        }

        [HttpGet("[action]")]
        public List<TableIndexInfo> LoadTableIndexes(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.LoadTableIndexes(astrtableName);
        }

        [HttpGet("[action]")]
        public TableCreateScript GetTableCreateScript(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableCreateScript(astrtableName);
        }

        [HttpGet("[action]")]
        public List<Tabledependencies> GetAllTabledependencies(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableDependencies(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableColumns> GetAllTablesColumn(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableColumns(astrtableName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetTableDescription(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableDescription(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableFKDependency> GetAllTableForeignKeys(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableForeignKeys(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrtableName, string astrdbName )
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableKeyConstraints(astrtableName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateColumnDescription(string astrTableName, string astrdbName, string astrDescription_Value,
            string astrColumnName)
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.CreateOrUpdateColumnDescription( astrDescription_Value,
                astrTableName.Split(".")[0], astrTableName, astrColumnName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableDescription(string astrTableName, string astrdbName, string astrDescription_Value)
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            ISrvMssql.CreateOrUpdateTableDescription( astrDescription_Value,astrTableName.Split(".")[0], astrTableName);
            return true;
        }

        [HttpGet("[action]")]
        public object GetDependancyTree(string astrtableName, string astrdbName)
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            var returnResult= JsonConvert.DeserializeObject( ISrvMssql.CreatorOrGetDependencyTree(astrtableName));
            return returnResult;
        }

        [HttpGet("[action]")]
        public List<TableFragmentationDetails> TableFragmentationDetails(string astrtableName, string astrdbName)
        {
            ISrvMssql.IstrDatabaseConnection = getActiveDatabaseInfo(astrdbName);
            return ISrvMssql.GetTableFragmentationDetails( astrtableName);
        }
    }
}