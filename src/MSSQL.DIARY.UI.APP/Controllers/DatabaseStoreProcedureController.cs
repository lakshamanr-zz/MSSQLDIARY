using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MoreLinq.Extensions;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.SRV;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MSSQL.DIARY.UI.APP.Data;
using Microsoft.AspNetCore.Identity;
using MSSQL.DIARY.UI.APP.Models;
using Microsoft.AspNetCore.Http;

namespace MSSQL.DIARY.UI.APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatabaseStoreProcedureController : ApplicationBaseController
    {
 

        public DatabaseStoreProcedureController()
        {
            SrvDatabaseStoreProc = new SrvDatabaseStoreProc();
            srvServerInfo = new SrvDatabaseServerInfo();
            
        }

        public DatabaseStoreProcedureController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvDatabaseStoreProc = new SrvDatabaseStoreProc();
            srvServerInfo = new SrvDatabaseServerInfo();
        }

        private SrvDatabaseStoreProc SrvDatabaseStoreProc { get; }
        private SrvDatabaseServerInfo srvServerInfo { get; }

        [HttpGet("[action]")]
        public List<SP_PropertyInfo> GetAllStoreprocedureDescription(string istrdbName, bool iblnSearchInSSISPackages)
        {
            var AllStoreprocedure = SrvDatabaseStoreProc.GetAllStoreprocedureDescription(istrdbName);
            var serverName = srvServerInfo.GetServerName().FirstOrDefault();
            //var SSRS_package = new List<PackageJsonHandler>();
            //SSISPackageInfoHandlerController.GetAllSSISPackages(_hostingEnv.WebRootPath);
            //SSISPackageInfoHandlerController.SSISPkgeCache.Cache.TryGetValue(serverName, out SSRS_package);
            //if (SSRS_package != null) FillSSISPackageDetails(AllStoreprocedure, SSRS_package);
            if (iblnSearchInSSISPackages)
                AllStoreprocedure = AllStoreprocedure.Where(x => x.lstSSISpackageReferance.IsNotNull()).ToList();
            return AllStoreprocedure;
        }

        //private static void FillSSISPackageDetails(List<SP_PropertyInfo> AllStoreprocedure,
        //    List<PackageJsonHandler> SSRS_package)
        //{
        //    AllStoreprocedure.ForEach(x =>
        //    {
        //        SSRS_package.ForEach(x1 =>
        //        {
        //            x1.ExecuteSQLTask.ForEach(x3 =>
        //            {
        //                if (x3.SqlStatementSource.Contains(x.istrName))
        //                {
        //                    if (x.lstSSISpackageReferance == null) x.lstSSISpackageReferance = new List<string>();

        //                    x.lstSSISpackageReferance.Add(x1.PackageLocation);
        //                }
        //            });
        //        });
        //        if (x.lstSSISpackageReferance != null)
        //            x.lstSSISpackageReferance = x.lstSSISpackageReferance.DistinctBy(x1 => x1).ToList();
        //    });
        //}

        [HttpGet("[action]")]
        public Ms_Description GetCreateScriptOfStoreProc(string istrdbName, string StoreprocName)
        {
            return SrvDatabaseStoreProc.GetCreateScriptOfStoreProc(istrdbName, StoreprocName);
        }

        [HttpGet("[action]")]
        public List<SP_Depencancy> GetStoreProcDependancy(string istrdbName, string StoreprocName)
        {
            return SrvDatabaseStoreProc.GetStoreProcDependancy(istrdbName, StoreprocName);
        }

        [HttpGet("[action]")]
        public List<SP_Parameters> GetStoreProcParameters(string istrdbName, string StoreprocName)
        {
            return SrvDatabaseStoreProc.GetStoreProcParameters(istrdbName, StoreprocName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetStoreProcMsDescription(string istrdbName, string StoreprocName)
        {
            return new Ms_Description
                {desciption = SrvDatabaseStoreProc.GetStoreProcMsDescription(istrdbName, StoreprocName)};
        }

        [HttpGet("[action]")]
        public List<ExecutionPlanInfo> GetCachedExecutionPlan(string istrdbName, string StoreprocName)
        {
            return SrvDatabaseStoreProc.GetCachedExecutionPlan(istrdbName, StoreprocName);
        }

        [HttpGet("[action]")]
        public object GetDependancyTree(string istrdbName, string StoreprocName)
        {
            return JsonConvert.DeserializeObject(
                SrvDatabaseStoreProc.CreatorOrGetDependancyTree(istrdbName, StoreprocName));
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateStoreProcParameterDescription(string istrdbName, string astrDescription_Value,
            string astrSP_Name, string astrSP_Parameter_Name)
        {
            SrvDatabaseStoreProc.CreateOrUpdateStoreProcParameterDescription(istrdbName, astrDescription_Value,
                astrSP_Name.Split(".")[0], astrSP_Name, astrSP_Parameter_Name);
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateStoreProcDescription(string istrdbName, string astrDescription_Value,
            string astrSP_Name)
        {
            SrvDatabaseStoreProc.CreateOrUpdateStoreProcDescription(istrdbName, astrDescription_Value,
                astrSP_Name.Split(".")[0], astrSP_Name);
        }
    }
}