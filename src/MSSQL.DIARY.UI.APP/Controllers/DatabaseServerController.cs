using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using Microsoft.AspNetCore.Authorization;
using MSSQL.DIARY.UI.APP.Data;
using Microsoft.AspNetCore.Identity;
using MSSQL.DIARY.UI.APP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DatabaseServerController : ApplicationBaseController
    {
        

        public DatabaseServerController(ILogger<DatabaseServerController> logger ,ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvServerInfo = new SrvMssql();
            this._logger = logger;
        }

        SrvMssql SrvServerInfo { get; }
        ILogger<DatabaseServerController> _logger { get; }

        [HttpGet("[action]")]
        public string GetServerInformation()
        {
            return getActiveServerName();
        }

        [HttpGet("[action]")]
        public List<string> GetDatabaseNames()
        {
            SrvServerInfo.IstrDatabaseConnection = getActiveDatabaseInfo();
            return SrvServerInfo.GetDatabaseNames();
        }
        [HttpGet("[action]")]
        public List<string> GetDatabaseNamesByServername(string astrServerName)
        {
            SrvServerInfo.IstrDatabaseConnection = GetConnectionString(astrServerName); 

            List<string> lstDatabaseName = new List<string>();
            lstDatabaseName.Add("Select Database");
            lstDatabaseName.AddRange(SrvServerInfo.GetDatabaseNames());
            return lstDatabaseName;
        }


        [HttpGet("[action]")]
        public List<PropertyInfo> GetServerProperties()
        {
            SrvServerInfo.IstrDatabaseConnection = getActiveDatabaseInfo();
            return SrvServerInfo.GetServerProperties();
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            SrvServerInfo.IstrDatabaseConnection = getActiveDatabaseInfo();
            return SrvServerInfo.GetAdvancedServerSettings();
        }

        [HttpGet("[action]")]
        public List<string> GetServerNameList()
        {
            return LoadServerList();
        }
        [HttpGet("[action]")]
        public string SetDefaultDatabase(string astrServerName, string astrDatabaseName)
        {
            setDefaultDatabaseActive(astrServerName, astrDatabaseName);
            return string.Empty;
        }

        [HttpGet("[action]")]
        public string GetDefaultDatabase()
        {
            try
            {
                return getActiveServerName() + ";" + getActiveDatabaseName();
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }
    }
}