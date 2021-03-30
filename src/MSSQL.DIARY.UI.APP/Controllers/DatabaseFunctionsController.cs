using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
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
    public class DatabaseFunctionsController : ApplicationBaseController
    {
         

        public DatabaseFunctionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvDatabaseScalarFunction = new SrvDatabaseFunctions("FN");
            SrvDatabaseTableValueFunction = new SrvDatabaseFunctions("TF");
        }

        public SrvDatabaseFunctions SrvDatabaseTableValueFunction { get; set; }
        public SrvDatabaseFunctions SrvDatabaseScalarFunction { get; set; }

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetScalerFunctionDependencies(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseScalarFunction.GetFunctionDependencies(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetScalerFunctionProperties(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseScalarFunction.GetFunctionProperties(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetScalerFunctionParameters(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseScalarFunction.GetFunctionParameters(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetScalerFunctionCreateScript(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseScalarFunction.GetFunctionCreateScript(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllScalarFunctionWithMsDescriptions(string istrdbName)
        {
            return SrvDatabaseScalarFunction.GetAllFunctionWithMsDescriptions(istrdbName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetScalarFunctionMsDescriptions(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseScalarFunction.GetFunctionMsDescriptions(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetTableValueFunctionDependencies(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseTableValueFunction.GetFunctionDependencies(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetTableValueFunctionProperties(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseTableValueFunction.GetFunctionProperties(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetTableValueFunctionParameters(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseTableValueFunction.GetFunctionParameters(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetTableValueFunctionCreateScript(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseTableValueFunction.GetFunctionCreateScript(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllTableValueFunctionWithMsDescriptions(string istrdbName)
        {
            return SrvDatabaseTableValueFunction.GetAllFunctionWithMsDescriptions(istrdbName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetTableValueFunctionMsDescriptions(string istrdbName, string astrFunctionName)
        {
            return SrvDatabaseTableValueFunction.GetFunctionMsDescriptions(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateScalerFunctionDescription(string istrdbName, string astrDescription_Value,
            string astrFunctionName)
        {
            SrvDatabaseScalarFunction.CreateOrUpdateFunctionDescription(istrdbName, astrDescription_Value,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableValueFunctionDescription(string istrdbName, string astrDescription_Value,
            string astrFunctionName)
        {
            SrvDatabaseTableValueFunction.CreateOrUpdateFunctionDescription(istrdbName, astrDescription_Value,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }
    }
}