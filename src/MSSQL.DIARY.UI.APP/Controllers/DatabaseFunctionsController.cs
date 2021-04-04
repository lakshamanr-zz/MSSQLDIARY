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
            SrvMssqlScalarFunction = new SrvMssql("FN");
            SrvMssqlTableValueFunction = new SrvMssql("TF");
        }

        public SrvMssql SrvMssqlTableValueFunction { get; set; }
        public SrvMssql SrvMssqlScalarFunction { get; set; }

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetScalerFunctionDependencies(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlScalarFunction.GetFunctionDependencies(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetScalerFunctionProperties(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlScalarFunction.GetFunctionProperties(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetScalerFunctionParameters(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlScalarFunction.GetFunctionParameters(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetScalerFunctionCreateScript(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlScalarFunction.GetFunctionCreateScript(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllScalarFunctionWithMsDescriptions(string istrdbName)
        {
            return SrvMssqlScalarFunction.GetFunctionsWithDescription(istrdbName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetScalarFunctionMsDescriptions(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlScalarFunction.GetFunctionWithDescription(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionDependencies> GetTableValueFunctionDependencies(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlTableValueFunction.GetFunctionDependencies(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionProperties> GetTableValueFunctionProperties(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlTableValueFunction.GetFunctionProperties(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<FunctionParameters> GetTableValueFunctionParameters(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlTableValueFunction.GetFunctionParameters(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public FunctionCreateScript GetTableValueFunctionCreateScript(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlTableValueFunction.GetFunctionCreateScript(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public List<PropertyInfo> GetAllTableValueFunctionWithMsDescriptions(string istrdbName)
        {
            return SrvMssqlTableValueFunction.GetFunctionsWithDescription(istrdbName);
        }

        [HttpGet("[action]")]
        public PropertyInfo GetTableValueFunctionMsDescriptions(string istrdbName, string astrFunctionName)
        {
            return SrvMssqlTableValueFunction.GetFunctionWithDescription(istrdbName, astrFunctionName);
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateScalerFunctionDescription(string istrdbName, string astrDescription_Value,
            string astrFunctionName)
        {
            SrvMssqlScalarFunction.CreateOrUpdateFunctionDescription(istrdbName, astrDescription_Value,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }

        [HttpGet("[action]")]
        public bool CreateOrUpdateTableValueFunctionDescription(string istrdbName, string astrDescription_Value,
            string astrFunctionName)
        {
            SrvMssqlTableValueFunction.CreateOrUpdateFunctionDescription(istrdbName, astrDescription_Value,
                astrFunctionName.Split(".")[0], astrFunctionName);
            return true;
        }
    }
}