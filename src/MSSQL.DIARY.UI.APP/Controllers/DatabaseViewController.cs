using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using Microsoft.AspNetCore.Authorization;
using MSSQL.DIARY.UI.APP.Data;
using Microsoft.AspNetCore.Identity;
using MSSQL.DIARY.UI.APP.Models;
using Microsoft.AspNetCore.Http;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DatabaseViewController : ApplicationBaseController
    {
        

        public DatabaseViewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvDatabaseViews = new SrvDatabaseViews();
        }

        private SrvDatabaseViews SrvDatabaseViews { get; }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllViewsDetails(string istrdbName)
        {
            string lstrDbConnection = getActiveDatabaseInfo(istrdbName);
            return SrvDatabaseViews.GetViewsWithDescription(lstrDbConnection);
        }

        [HttpGet("[action]")]
        public List<ViewDependancy> GetView_Dependancies(string istrdbName, string astrViewName)
        {
            return SrvDatabaseViews.GetViewDependencies(istrdbName, astrViewName);
        }

        [HttpGet("[action]")]
        public List<View_Properties> GetViewProperties(string istrdbName, string astrViewName)
        {
            return SrvDatabaseViews.GetViewProperties(istrdbName, astrViewName);
        }

        [HttpGet("[action]")]
        public List<ViewColumns> GetViewColumns(string istrdbName, string astrViewName)
        {
            return SrvDatabaseViews.GetViewColumns(istrdbName, astrViewName);
        }

        [HttpGet("[action]")]
        public ViewCreateScript GetViewCreateScript(string istrdbName, string astrViewName)
        {
            return SrvDatabaseViews.GetViewCreateScript(istrdbName, astrViewName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetViewNameWithMs_description(string istrdbName, string astrViewName)
        {
            return SrvDatabaseViews.GetViewsWithDescription(istrdbName, astrViewName);
        }
    }
}