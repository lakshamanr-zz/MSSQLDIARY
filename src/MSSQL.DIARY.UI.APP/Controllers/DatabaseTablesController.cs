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
            srvDatabaseTable = new SrvDatabaseTable(); 
            this.applicationDbContext = context;
        }
        private SrvDatabaseTable srvDatabaseTable { get; }

        [HttpGet("[action]")]
        public List<TablePropertyInfo> GetAllDatabaseTable(string astrdbName)
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName); 
           return srvDatabaseTable.GetAllDatabaseTablesDescription(); 
        }

        [HttpGet("[action]")]
        public List<TableIndexInfo> LoadTableIndexes(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.LoadTableIndexes(astrtableName);
        }

        [HttpGet("[action]")]
        public TableCreateScript GetTableCreateScript(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetTableCreateScript(astrtableName);
        }

        [HttpGet("[action]")]
        public List<Tabledependencies> GetAllTabledependencies(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetAllTabledependencies(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableColumns> GetAllTablesColumn(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetAllTablesColumn(astrtableName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetTableDescription(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetTableDescription(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableFKDependency> GetAllTableForeignKeys(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetAllTableForeignKeys(astrtableName);
        }

        [HttpGet("[action]")]
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrtableName, string astrdbName )
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.GetTableKeyConstraints(astrtableName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateColumnDescription(string astrTableName, string astrdbName, string astrDescription_Value,
            string astrColumnName)
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.CreateOrUpdateColumnDescription( astrDescription_Value,
                astrTableName.Split(".")[0], astrTableName, astrColumnName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableDescription(string astrTableName, string astrdbName, string astrDescription_Value)
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            srvDatabaseTable.CreateOrUpdateTableDescription( astrDescription_Value,astrTableName.Split(".")[0], astrTableName);
            return true;
        }

        [HttpGet("[action]")]
        public object GetDependancyTree(string astrtableName, string astrdbName)
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            var returnResult= JsonConvert.DeserializeObject( srvDatabaseTable.CreatorOrGetDependancyTree(astrtableName));
            return returnResult;
        }

        [HttpGet("[action]")]
        public List<TableFragmentationDetails> TableFragmentationDetails(string astrtableName, string astrdbName)
        {
            srvDatabaseTable.istrDBConnection = getActiveDatabaseInfo(astrdbName);
            return srvDatabaseTable.TableFragmentationDetails( astrtableName);
        }
    }
}