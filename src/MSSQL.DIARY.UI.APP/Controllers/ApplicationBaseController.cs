using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;

namespace MSSQL.DIARY.UI.APP.Controllers
{

     
    public class ApplicationBaseController : ControllerBase
    {
        static Dictionary<string, string> naiveCache = new  Dictionary<string, string>();
        public ApplicationBaseController()
        { 
        }
        public ApplicationBaseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        { 
            _context = context;
            _userManager = userManager; 
            _httpContextAccessor = httpContextAccessor;

        }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        [NonAction]
        public void setActiveDatabaseInfo(string DatabaseNameConnection)
        {
             string UserId = GetUserId();
            if (!naiveCache.ContainsKey(UserId))
            {
                try
                {
                    naiveCache.Add(UserId, DatabaseNameConnection);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            
        }
        [NonAction]
        public void RemmoveActiveDatabaseInfo()
        {
            string UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                naiveCache.Remove(UserId);
            }
        }
        [NonAction]
        public string getActiveDatabaseInfo()
        {
           var  UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                 return naiveCache.Where(x => x.Key.Equals(UserId)).FirstOrDefault().Value;
            }
            return string.Empty;
        }
        [NonAction]
        public string getActiveDatabaseName()
        {
            string UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                return naiveCache.Where(x => x.Key.Equals(UserId)).FirstOrDefault().Value.Split(';')[1].Replace(" Initial Catalog =", "").Replace(" Initial Catalog  =", "");
            }
            return string.Empty;
        }
        [NonAction]
        public string getActiveDatabaseInfo(string istrdbName)
        {
            var UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                var conn= naiveCache.Where(x => x.Key.Equals(UserId)).FirstOrDefault().Value;
                var DatabaseConnection = string.Empty; 
                DatabaseConnection += conn.Split(';')[0]+";";
                DatabaseConnection += $"Database={istrdbName};";
                DatabaseConnection +=conn.Split(';')[2] + ";";
                DatabaseConnection += conn.Split(';')[3] + ";";
                DatabaseConnection += "Trusted_Connection=false;";
                return DatabaseConnection; 
            }
            return string.Empty;
        }
        [NonAction]
        public string getActiveServerName(string UserId)
        {
            if (naiveCache.ContainsKey(UserId))
            {
                return naiveCache.Where(x => x.Key.Equals(UserId)).FirstOrDefault().Value.Split(';')[0].Replace("server=", "");
            }
            return string.Empty;
        }
        [NonAction]
        public string getActiveServerName( )
        {
            string UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                return naiveCache.Where(x => x.Key.Equals(UserId)).FirstOrDefault().Value.Split(';')[0].Replace("Server=", "").Replace("Data Source =", "").Replace("Data Source  =", "");
            }
            return string.Empty;
        }
        [NonAction]
        public void setDefaultDatabaseActive(string astrServerName, string astrDatabaseName)
        {
            string DatabaseConnection = CreateConnectionString(astrServerName, astrDatabaseName);
            string UserId = GetUserId();
            if (naiveCache.ContainsKey(UserId))
            {
                naiveCache.Remove(UserId);
            }
            naiveCache.Add(UserId, DatabaseConnection);

        }
        [NonAction]
        public string GetDefaultConnectionString()
        {
            string UserId = GetUserId();
            string lstrServerNameAndDatabase = string.Empty;
            var DatabaseDetails = _context.UserDatabases.FirstOrDefault(x => x.UserId.Contains(UserId));

            var DatabaseConnection = string.Empty;
            if (DatabaseDetails.IsNotNull())
            {
                DatabaseConnection += DatabaseDetails.DatabaseServerName + ";";
                DatabaseConnection += DatabaseDetails.DatabaseName + ";";
                DatabaseConnection += DatabaseDetails.DatabaseUserName + ";";
                DatabaseConnection += DatabaseDetails.DatabasePassword + ";";
                DatabaseConnection += "Trusted_Connection=false;";
            }
            return DatabaseConnection;
        }
        [NonAction]
        public string GetConnectionString(string DatabaseServerName, string astrDatabaseName = null)
        {
            string UserId = GetUserId();
            UserDatabases DatabaseDetails = null;
            if (astrDatabaseName.IsNotNullOrEmpty())
            {
                DatabaseDetails = _context.UserDatabases.Where(x =>
                                      x.UserId.Contains(UserId) &&
                                      x.DatabaseServerName.Contains(DatabaseServerName) &&
                                      x.DatabaseName.Contains(astrDatabaseName)
                                ).FirstOrDefault();
            }
            if (DatabaseDetails.IsNull())
            {
                DatabaseDetails = _context.UserDatabases.Where(x =>
                                                      x.UserId.Contains(UserId) &&
                                                      x.DatabaseServerName.Contains(DatabaseServerName)
                                                ).FirstOrDefault();


            }


            var DatabaseConnection = string.Empty;
            if (DatabaseDetails.IsNotNull())
            {
                DatabaseConnection += DatabaseDetails.DatabaseServerName + ";";
                DatabaseConnection += DatabaseDetails.DatabaseName + ";";
                DatabaseConnection += DatabaseDetails.DatabaseUserName + ";";
                DatabaseConnection += DatabaseDetails.DatabasePassword + ";";
                DatabaseConnection += "Trusted_Connection=false;";
            }

            return DatabaseConnection;
        }
        [NonAction]
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type.Contains("nameidentifier")).FirstOrDefault().Value;
        }
        [NonAction]
        public List<string> LoadServerList()
        {
            string UserId = GetUserId();
            List<string> lstrServerNames = new List<string>();
            lstrServerNames.Add("Select Server");
            foreach (var lstrServerName in _context.UserDatabases.Where(x => x.UserId.Contains(UserId)))
            {
                lstrServerNames.Add(lstrServerName.DatabaseServerName.Replace("Data Source =", "").Replace("Data Source=", ""));
            }
            return lstrServerNames;
        }


        [NonAction]
        public string CreateConnectionString(string DatabaseServerName, string astrDatabaseName = null)
        {
            string UserId = GetUserId();
            var DatabaseDetails = _context.UserDatabases.FirstOrDefault(x => x.DatabaseServerName.Contains(DatabaseServerName) &&  x.UserId.Contains(UserId));
            var DatabaseConnection = string.Empty;
            if (DatabaseDetails.IsNotNull())
            {
                DatabaseConnection += DatabaseDetails.DatabaseServerName+";";
                DatabaseConnection += " Initial Catalog =" + astrDatabaseName + ";";
                DatabaseConnection += DatabaseDetails.DatabaseUserName + ";";
                DatabaseConnection += DatabaseDetails.DatabasePassword + ";";
                DatabaseConnection += "Trusted_Connection=false;";

            }
            return DatabaseConnection;
        }
  
    }
}
