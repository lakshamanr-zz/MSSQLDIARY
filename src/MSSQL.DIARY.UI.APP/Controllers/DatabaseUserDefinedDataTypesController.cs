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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatabaseUserDefinedDataTypesController : ApplicationBaseController
    {
        public DatabaseUserDefinedDataTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, userManager, httpContextAccessor)
        {
            SrvDatabaseUserDefinedDataTypes = new SrvMssql();
        }

        private SrvMssql SrvDatabaseUserDefinedDataTypes { get; }

        [HttpGet("[action]")]
        public List<UserDefinedDataTypeDetails> GetAllUserDefinedDataTypes(string istrdbName)
        {
            return SrvDatabaseUserDefinedDataTypes.GetUserDefinedDataTypes(istrdbName);
        }

        [HttpGet("[action]")]
        public UserDefinedDataTypeDetails GetUserDefinedDataTypeDetails(string istrdbName, string istrTypeName)
        {
            return SrvDatabaseUserDefinedDataTypes.GetUserDefinedDataType(istrdbName, istrTypeName);
        }

        [HttpGet("[action]")]
        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReferance(string istrdbName,
            string istrTypeName)
        {
            return SrvDatabaseUserDefinedDataTypes.GetUsedDefinedDataTypeReference(istrdbName, istrTypeName);
        }

        [HttpGet("[action]")]
        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string istrdbName, string istrTypeName)
        {
            return SrvDatabaseUserDefinedDataTypes.GetUsedDefinedDataTypeExtendedProperties(istrdbName, istrTypeName);
        }

        [HttpGet("[action]")]
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string istrdbName, string istrTypeName,
            string istrdescValue)
        {
            SrvDatabaseUserDefinedDataTypes.CreateOrUpdateUsedDefinedDataTypeExtendedProperties(istrdbName,
                istrTypeName, istrdescValue);
        }
    }
}