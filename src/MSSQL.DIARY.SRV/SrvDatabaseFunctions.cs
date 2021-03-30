using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseFunctions
    {
        public SrvDatabaseFunctions(string function_type)
        {
            this.function_type = function_type;
        }

        public string function_type { get; set; }

        public List<FunctionDependencies> GetFunctionDependencies(string istrdbConn, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetFunctionDependencies(astrFunctionName, function_type).DistinctBy(x => x.name)
                    .ToList();
            }
        }

        public List<FunctionProperties> GetFunctionProperties(string istrdbConn, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetFunctionProperties(astrFunctionName, function_type);
            }
        }

        public List<FunctionParameters> GetFunctionParameters(string istrdbConn, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetFunctionParameters(astrFunctionName, function_type);
            }
        }

        public FunctionCreateScript GetFunctionCreateScript(string istrdbConn, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetFunctionCreateScript(astrFunctionName, function_type);
            }
        }

        public List<PropertyInfo> GetAllFunctionWithMsDescriptions(string istrdbConn)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetAllFunctionWithMsDescriptions(function_type);
            }
        }

        public PropertyInfo GetFunctionMsDescriptions(string istrdbConn, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetAllFunctionWithMsDescriptions(function_type)
                    .FirstOrDefault(x => x.istrName.Contains(astrFunctionName)) ?? new PropertyInfo
                    {istrName = astrFunctionName, istrValue = ""};
            }
        }

        public void CreateOrUpdateFunctionDescription(string istrdbConn, string astrDescription_Value,
            string astrSchema_Name, string astrFunctionName)
        {
            using (var dbSqldocContext = new MssqlDiaryContext(istrdbConn))
            {
                dbSqldocContext.CreateOrUpdateFunctionDescription(astrDescription_Value, astrSchema_Name,
                    astrFunctionName);
            }
        }
    }
}