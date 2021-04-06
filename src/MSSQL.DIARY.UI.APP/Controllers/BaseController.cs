using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;
using System.Collections.Generic;
using System.Linq;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    public class BaseController : ControllerBase
    {
        static readonly Dictionary<string, string> NaiveCache = new  Dictionary<string, string>();
        public BaseController()
        { 
        }
        public BaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        { 
            _context = context;
            Manager = userManager; 
            _httpContextAccessor = httpContextAccessor;

        }

        public UserManager<ApplicationUser> Manager { get; }
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        [NonAction]
        public string GetActiveDatabaseInfo()
        {
           var  userId = GetUserId();
            if (NaiveCache.ContainsKey(userId))
            {
                 return NaiveCache.FirstOrDefault(x => x.Key.Equals(userId)).Value;
            }
            return string.Empty;
        }
        [NonAction]
        public string GetActiveDatabaseName()
        {
            string userId = GetUserId();
            if (NaiveCache.ContainsKey(userId))
            {
                return NaiveCache.FirstOrDefault(x => x.Key.Equals(userId)).Value.Split(';')[1].Replace(" Initial Catalog =", "").Replace(" Initial Catalog  =", "");
            }
            return string.Empty;
        }
        [NonAction]
        public string GetActiveDatabaseInfo(string istrdbName)
        {
            var userId = GetUserId();
            if (NaiveCache.ContainsKey(userId))
            {
                var conn= NaiveCache.FirstOrDefault(x => x.Key.Equals(userId)).Value;
                var lstrdatabaseConnection = string.Empty; 
                lstrdatabaseConnection += conn.Split(';')[0]+";";
                lstrdatabaseConnection += $"Database={istrdbName};";
                lstrdatabaseConnection +=conn.Split(';')[2] + ";";
                lstrdatabaseConnection += conn.Split(';')[3] + ";";
                lstrdatabaseConnection += "Trusted_Connection=false;";
                return lstrdatabaseConnection; 
            }
            return string.Empty;
        }

        [NonAction]
        public string GetActiveServerName( )
        {
            string userId = GetUserId();
            if (NaiveCache.ContainsKey(userId))
            {
                return NaiveCache.FirstOrDefault(x => x.Key.Equals(userId)).Value.Split(';')[0].Replace("Server=", "").Replace("Data Source =", "").Replace("Data Source  =", "");
            }
            return string.Empty;
        }
        [NonAction]
        public void SetDefaultDatabaseActive(string astrServerName, string astrDatabaseName)
        {
            string lstrDatabaseConnection = CreateConnectionString(astrServerName, astrDatabaseName);
            string userId = GetUserId();
            if (NaiveCache.ContainsKey(userId))
            {
                NaiveCache.Remove(userId);
            }
            NaiveCache.Add(userId, lstrDatabaseConnection);

        }

        [NonAction]
        public string GetConnectionString(string astrServerName, string astrDatabaseName = null)
        {
            string userId = GetUserId();
            UserDatabases databaseDetails = null;
            if (astrDatabaseName.IsNotNullOrEmpty())
            {
                databaseDetails = _context.UserDatabases.FirstOrDefault(x => x.UserId.Contains(userId) && x.DatabaseServerName.Contains(astrServerName) && x.DatabaseName.Contains(astrDatabaseName));
            }
            if (databaseDetails.IsNull())
            {
                databaseDetails = _context.UserDatabases.FirstOrDefault(x => x.UserId.Contains(userId) && x.DatabaseServerName.Contains(astrServerName));

            }

            var lstrDatabaseConnection = string.Empty;
            if (databaseDetails.IsNotNull())
            {
                if (databaseDetails != null)
                {
                    lstrDatabaseConnection += databaseDetails.DatabaseServerName + ";";
                    lstrDatabaseConnection += databaseDetails.DatabaseName + ";";
                    lstrDatabaseConnection += databaseDetails.DatabaseUserName + ";";
                    lstrDatabaseConnection += databaseDetails.DatabasePassword + ";";
                }

                lstrDatabaseConnection += "Trusted_Connection=false;";
            }

            return lstrDatabaseConnection;
        }
        [NonAction]
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier"))?.Value;
        }
        [NonAction]
        public List<string> LoadServerList()
        {
            string userId = GetUserId();
            var lstrServerNames = new List<string>();
            lstrServerNames.Add("Select Server");
            foreach (var lstrServerName in _context.UserDatabases.Where(x => x.UserId.Contains(userId)))
            {
                lstrServerNames.Add(lstrServerName.DatabaseServerName.Replace("Data Source =", "").Replace("Data Source=", ""));
            }
            return lstrServerNames;
        }
        [NonAction]
        public string CreateConnectionString(string astrServerName, string astrDatabaseName = null)
        {
            string userId = GetUserId();
            var databaseDetails = _context.UserDatabases.FirstOrDefault(x => x.DatabaseServerName.Contains(astrServerName) &&  x.UserId.Contains(userId));
            var lstrDatabaseConnection = string.Empty;
            
                if (databaseDetails != null)
                {
                    lstrDatabaseConnection += databaseDetails.DatabaseServerName + ";";
                    lstrDatabaseConnection += " Initial Catalog =" + astrDatabaseName + ";";
                    lstrDatabaseConnection += databaseDetails.DatabaseUserName + ";";
                    lstrDatabaseConnection += databaseDetails.DatabasePassword + ";";
                }

                lstrDatabaseConnection += "Trusted_Connection=false;";

            
            return lstrDatabaseConnection;
        }
  
    }
}
