using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSSQL.DIARY.SRV;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;
using Newtonsoft.Json;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DatabaseObjectExplorerController : ApplicationBaseController
    {
   
       public DatabaseObjectExplorerController(ILogger<DatabaseObjectExplorerController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) :base(context,userManager, httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _srvServerInfo = new SrvMssql();
            _httpContextAccessor = httpContextAccessor;
          
        }
        private SrvMssql _srvServerInfo { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseObjectExplorerController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string istrDatabaseName { get; set; }




        [HttpGet]
        public string GetObjectExplorerDetails(string DatabaseServerName, string astrDatabaseName)
        {  
            var dbConnection = getActiveDatabaseInfo(); 

            if (dbConnection.IsNotNullOrEmpty())
            {

                return SrvMssql.ObjectExplorerDetails.GetOrCreate(dbConnection, GetObjectExplorer);

            }
            
            if (SrvMssql.ObjectExplorerDetails.Cache.Count > 0)
            {

                return SrvMssql.ObjectExplorerDetails.GetOrCreate(dbConnection, GetObjectExplorer);
            }
            return string.Empty; 

        } 
        private string GetObjectExplorer()
        {
            return @"{""data"":" + JsonConvert.SerializeObject(SrvMssql.GetObjectExplorer(getActiveDatabaseInfo(), getActiveDatabaseName())) + "}";
        }
    }
}
