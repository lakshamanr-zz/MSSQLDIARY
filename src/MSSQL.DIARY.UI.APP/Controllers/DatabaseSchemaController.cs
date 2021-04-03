using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatabaseSchemaController : ApplicationBaseController
    {
        
        public DatabaseSchemaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            srvDatabaseSchema = new SrvDatabaseSchema();
        }

        private SrvDatabaseSchema srvDatabaseSchema { get; }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetListOfAllSchemaAndMsDescription(string istrdbName)
        {
            return srvDatabaseSchema.GetSchemaWithDescriptions(istrdbName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> CreateOrUpdateSchemaMsDescription(string istrdbName)
        {
            return srvDatabaseSchema.GetSchemaWithDescriptions(istrdbName);
        }

        [HttpGet("[action]")]
        public List<SchemaReferanceInfo> GetSchemaReferanceInfo(string istrdbName, string astrSchema_Name)
        {
            return srvDatabaseSchema.GetSchemaReferences(istrdbName, astrSchema_Name);
        }

        [HttpGet("[action]")]
        public Ms_Description GetSchemaMsDescription(string istrdbName, string astrSchema_Name)
        {
            return srvDatabaseSchema.GetSchemaDescription(istrdbName, astrSchema_Name);
        }
    }
}