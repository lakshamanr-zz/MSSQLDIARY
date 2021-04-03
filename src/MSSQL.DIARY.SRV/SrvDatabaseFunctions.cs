using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseFunctions
    {
        public SrvDatabaseFunctions(string astrFunctionType)
        {
            this.IstrFunctionType = astrFunctionType;
        }

        public string IstrFunctionType { get; set; }

        
        public List<FunctionDependencies> GetFunctionDependencies(string astrDatabaseConnection, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionDependencies(astrFunctionName, IstrFunctionType).DistinctBy(x => x.name).ToList();
            }
        }

        /// <summary>
        /// Get the function properties.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public List<FunctionProperties> GetFunctionProperties(string astrDatabaseConnection, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionProperties(astrFunctionName, IstrFunctionType);
            }
        }

        /// <summary>
        /// Get function parameters.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>

        public List<FunctionParameters> GetFunctionParameters(string astrDatabaseConnection, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionParameters(astrFunctionName, IstrFunctionType);
            }
        }

        /// <summary>
        /// Get function create script
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public FunctionCreateScript GetFunctionCreateScript(string astrDatabaseConnection, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionCreateScript(astrFunctionName, IstrFunctionType);
            }
        }

        /// <summary>
        /// Get functions with it descriptions
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetFunctionsWithDescription(string astrDatabaseConnection)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionsWithDescription(IstrFunctionType);
            }
        }
        /// <summary>
        /// Get function with description
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public PropertyInfo GetFunctionWithDescription(string astrDatabaseConnection, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetFunctionsWithDescription(IstrFunctionType).FirstOrDefault(x => x.istrName.Contains(astrFunctionName)) ?? new PropertyInfo {istrName = astrFunctionName, istrValue = ""};
            }
        }

        /// <summary>
        /// Create or update function description
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrFunctionName"></param>
        public void CreateOrUpdateFunctionDescription(string astrDatabaseConnection, string astrDescriptionValue, string astrSchemaName, string astrFunctionName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                dbSqldocContext.CreateOrUpdateFunctionDescription(astrDescriptionValue, astrSchemaName, astrFunctionName);
            }
        }
    }
}