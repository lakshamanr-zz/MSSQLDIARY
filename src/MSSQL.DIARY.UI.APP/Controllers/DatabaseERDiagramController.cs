using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.UI.APP.Data;
using MSSQL.DIARY.UI.APP.Models;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatabaseERDiagramController : ApplicationBaseController
    {
        public DatabaseERDiagramController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
        }
        [HttpGet("[action]")]
        public Ms_Description GetERDiagram(string istrdbName, string istrServerName, string istrSchemaName)
        {
            var result = new Ms_Description();
            //if (istrSchemaName.Equals("All"))
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName, istrServerName, null)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName.Split('/')[0],
            //            istrdbName.Split('/')[1], null);
            //else
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName, istrServerName, istrSchemaName)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName.Split('/')[0],
            //            istrdbName.Split('/')[1], istrSchemaName);


            return result;
        }
        [HttpGet("[action]")]
        public Ms_Description GetERDiagramWithSelectedTables(string istrdbName, string istrServerName, string istrSchemaName, string SelectedTables)
        {
            var alstOfSelectedTables = SelectedTables.Split(';').Where(x => x.IsNotNullOrEmpty()).ToList();
            var newSelectedTables = new List<string>();
            alstOfSelectedTables.ForEach(x => {
                newSelectedTables.Add(x.Split('.')[1]);
            });
            var result = new Ms_Description();
            //if (istrSchemaName.Equals("All"))
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName, istrServerName, null, newSelectedTables)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName.Split('/')[0],
            //            istrdbName.Split('/')[1], null, newSelectedTables);
            //else
            //    result.desciption = !istrServerName.IsNullOrEmpty()
            //        ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName, istrServerName, istrSchemaName, newSelectedTables)
            //        : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName.Split('/')[0],
            //            istrdbName.Split('/')[1], istrSchemaName, newSelectedTables);


            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description SaveERDiagramWithSelectedTables(string istrdbName, string istrServerName, string SelectedTables, string istrsqlmodule)
        {

            var result = new Ms_Description();

            //DatabaseModule databaseModule = new DatabaseModule();
            //databaseModule.DatabaseName = istrdbName;
            //databaseModule.ServerName = istrServerName;
            //databaseModule.tables = SelectedTables;
            //databaseModule.DbModuleName = istrsqlmodule;
            //if (!applicationDbContext.databaseModule
            //    .Where(x => x.DatabaseName.Contains(istrdbName) && x.ServerName.Contains(istrServerName) && x.DbModuleName.Contains(istrsqlmodule))
            //    .Any())
            //{
            //    applicationDbContext.databaseModule.Add(databaseModule);
            //    applicationDbContext.SaveChanges();
            //    result.desciption = "Module save successfully ";
            //}
            //else
            //{
            //    result.desciption = "There is already same name module is existing in database";
            //}
            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description LoadERDiagramWithSelectedTables(string istrdbName, string istrServerName, string istrsqlmodule)
        {

            var result = new Ms_Description();
            //var sqlmodule =applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(istrdbName) &&
            //     x.ServerName.Contains(istrServerName) &&
            //     x.DbModuleName.Contains(istrsqlmodule)

            //     ).FirstOrDefault() ;
            // if (sqlmodule.IsNotNull())
            // {
            //     var alstOfSelectedTables = sqlmodule.tables.Split(';').Where(x => x.IsNotNullOrEmpty()).ToList();
            //     var newSelectedTables = new List<string>();
            //     alstOfSelectedTables.ForEach(x =>
            //     {
            //         newSelectedTables.Add(x.Split('.')[1]);
            //     });
            //     result.desciption = !istrServerName.IsNullOrEmpty()
            //         ? SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName, istrServerName, null, newSelectedTables)
            //         : SrvDatabaseInfo.GetERDiagram(_hostingEnv.WebRootPath, istrdbName.Split('/')[0],
            //             istrdbName.Split('/')[1], null, newSelectedTables);
            // } 

            return result;

        }
        [HttpGet("[action]")]
        public Ms_Description deleteERDiagramWithSelectedTables(string istrdbName, string istrServerName, string istrsqlmodule)
        {

            var result = new Ms_Description();
            //var sqlmodule = applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(istrdbName) &&
            //     x.ServerName.Contains(istrServerName) &&
            //     x.DbModuleName.Contains(istrsqlmodule)

            //     ).FirstOrDefault();
            //if (sqlmodule.IsNotNull())
            //{
            //    applicationDbContext.databaseModule.Remove(sqlmodule);
            //    applicationDbContext.SaveChanges();
            //}

            return result;

        }
        [HttpGet("[action]")]
        public List<string> LoadAllModuless(string istrdbName, string istrServerName)
        {

            var result = new List<string>();
            //var sqlmodule = applicationDbContext.databaseModule.Where(
            //     x => x.DatabaseName.Contains(istrdbName) &&
            //     x.ServerName.Contains(istrServerName));
            //if (sqlmodule.IsNotNull())
            //{
            //    result = sqlmodule.Select(x => x.DbModuleName).ToList();
            //}

            return result;

        }
    }
}
