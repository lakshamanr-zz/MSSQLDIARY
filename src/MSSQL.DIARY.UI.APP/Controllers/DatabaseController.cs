using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using System.Linq;
using MSSQL.DIARY.UI.APP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MSSQL.DIARY.UI.APP.Models;
using Microsoft.AspNetCore.Http;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DatabaseController : ApplicationBaseController
    {

        private readonly ApplicationDbContext applicationDbContext;


        public DatabaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvDatabaseInfo = new SrvDatabaseInfo();
            this.applicationDbContext = context;
        }

        private SrvDatabaseInfo SrvDatabaseInfo { get; }

        [HttpGet("[action]")]
        public string GetDatabaseUserDefinedText()
        {
            return "";
        }

        [HttpGet("[action]")]
        public List<string> GetDatabaseObjectTypes()
        {
            return SrvDatabaseInfo.GetDatabaseObjectTypes();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetdbPropertValues(string istrdbName)
        {
            SrvDatabaseInfo.istrDatabaseConnection = getActiveDatabaseInfo(istrdbName);
            return SrvDatabaseInfo.GetDatabaseProperties();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetdbOptionValues(string istrdbName)
        {
            SrvDatabaseInfo.istrDatabaseConnection = getActiveDatabaseInfo(istrdbName);
            return SrvDatabaseInfo.GetDatabaseOptions();
        }

        [HttpGet("[action]")]
        public List<FileInfomration> GetdbFilesDetails(string istrdbName)
        {
            SrvDatabaseInfo.istrDatabaseConnection = getActiveDatabaseInfo(istrdbName); 
            return SrvDatabaseInfo.GetDatabaseFiles();
        }
    }
}