using System.Collections.Generic;
using System.Linq;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseStoreProc
    {
        public static NaiveCache<List<SP_PropertyInfo>> StoreProcedureDescription = new NaiveCache<List<SP_PropertyInfo>>();

        public static NaiveCache<List<ExecutionPlanInfo>> CacheExecutionPlan = new NaiveCache<List<ExecutionPlanInfo>>();

        public static NaiveCache<string> CacheThatDependsOn = new NaiveCache<string>();

        public List<SP_PropertyInfo> GetStoreProceduresWithDescription(string astrDatabaseName)
        {
            return StoreProcedureDescription.GetOrCreate(astrDatabaseName + "GetStoreProceduresWithDescription", () => GetStoreProcedureFromCache(astrDatabaseName));
        }

        private List<SP_PropertyInfo> GetStoreProcedureFromCache(string astrDatabaseName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                var lstStoreProcedureDetails = new List<SP_PropertyInfo>();
                foreach (var storeProcedureKeyValuePair in dbSqldocContext.GetStoreProceduresWithDescription().GroupBy(x => x.istrName))
                {
                    var lstrStoreProcedureDetails = new SP_PropertyInfo {istrName = storeProcedureKeyValuePair.Key};

                    foreach (var value in storeProcedureKeyValuePair)
                    {
                        if (value.istrValue.IsNotNullOrEmpty())
                            lstrStoreProcedureDetails.istrValue += value.istrValue;
                    }    
                    if (lstrStoreProcedureDetails.istrValue.IsNullOrEmpty())
                        lstrStoreProcedureDetails.istrValue += "Description of " + lstrStoreProcedureDetails.istrName + " is Empty ";
                    lstStoreProcedureDetails.Add(lstrStoreProcedureDetails);
                }

                if (!lstStoreProcedureDetails.Any())
                {
                    lstStoreProcedureDetails.Add(new SP_PropertyInfo { istrName = "", istrValue = "" });
                } 

                lstStoreProcedureDetails.ForEach(x =>
                {
                    x.lstrCreateScript = new List<string>
                    {
                        GetStoreProcedureCreateScript(astrDatabaseName, x.istrName).desciption
                    };
                });

                return lstStoreProcedureDetails;
            }
        }
        /// <summary>
        /// Get create script of the store procedure
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public Ms_Description GetStoreProcedureCreateScript(string astrDatabaseName, string astrStoreProcedureName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetStoreProcedureCreateScript(astrStoreProcedureName);
            }
        }
        /// <summary>
        /// Get store procedure dependencies
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<SP_Depencancy> GetStoreProceduresDependency(string astrDatabaseName, string astrStoreProcedureName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetStoreProceduresDependency(astrStoreProcedureName);
            }
        }

        /// <summary>
        /// Store the store procedure dependency tree and if already present then load 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public string CreatorOrGetStoreProcedureDependencyTree(string astrDatabaseName, string astrStoreProcedureName)
        {
            return CacheThatDependsOn.GetOrCreate(astrDatabaseName + "StoreProcedure" + astrStoreProcedureName,() => CreateOrGetCacheStoreProcedureThatDependsOn(astrDatabaseName, astrStoreProcedureName));
        }

        /// <summary>
        /// Get store procedure that dependent on
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string CreateOrGetCacheStoreProcedureThatDependsOn(string astrDatabaseName, string astrObjectName)
        {
            SrvDatabaseObjectDependency lSrvDatabaseObjectDependency = new SrvDatabaseObjectDependency();
            string lstrStoreProcedureThatDependsOn = lSrvDatabaseObjectDependency.GetObjectThatDependsOn(astrDatabaseName, astrObjectName);
            string lstrStoreProcedureOnWhichDepends = lSrvDatabaseObjectDependency.GetObjectOnWhichDepends(astrDatabaseName, astrObjectName);
            return lSrvDatabaseObjectDependency.GetJsonResult(lstrStoreProcedureOnWhichDepends, lstrStoreProcedureThatDependsOn, astrObjectName);
        }
         
        /// <summary>
        /// Get store procedure parameter with descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<SP_Parameters> GetStoreProceduresParametersWithDescription(string astrDatabaseName, string astrStoreProcedureName)
        {
            var lstStoreProcedureParameters = new List<SP_Parameters>();
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                foreach (var storeProceduresParameterKey in dbSqldocContext.GetStoreProceduresParametersWithDescription(astrStoreProcedureName).GroupBy(x => x.Parameter_name))
                {
                    var lStoreProcedureParameter = new SP_Parameters {Parameter_name = storeProceduresParameterKey.Key};
                    foreach (var storeProceduresParameterValue in storeProceduresParameterKey)
                    {
                        lStoreProcedureParameter.Parameter_name = storeProceduresParameterValue.Parameter_name;
                        lStoreProcedureParameter.Type = storeProceduresParameterValue.Type;
                        lStoreProcedureParameter.Length = storeProceduresParameterValue.Length;
                        lStoreProcedureParameter.Prec = storeProceduresParameterValue.Prec;
                        lStoreProcedureParameter.Scale = storeProceduresParameterValue.Scale;
                        lStoreProcedureParameter.Param_order = storeProceduresParameterValue.Param_order;
                        lStoreProcedureParameter.Extended_property += storeProceduresParameterValue.Extended_property;
                    } 
                    lstStoreProcedureParameters.Add(lStoreProcedureParameter);
                }
            }

            return lstStoreProcedureParameters;
        }

        /// <summary>
        /// Get store procedure execution plan details
        /// </summary>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<ExecutionPlanInfo> GetStoreProcedureExecutionPlan(string astrDatabaseName, string astrStoreProcedureName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                var lstExecutionPlanDetails = dbSqldocContext.GetStoreProcedureExecutionPlan(astrStoreProcedureName);

                if (lstExecutionPlanDetails.Any())
                {
                    return CacheExecutionPlan.GetOrCreate(astrDatabaseName + astrStoreProcedureName, () => lstExecutionPlanDetails);
                }

                if (CacheExecutionPlan.Cache.TryGetValue(astrDatabaseName + astrStoreProcedureName, out var executionPlanDetails)) 
                    return executionPlanDetails;
                return lstExecutionPlanDetails;
            }
        }
        /// <summary>
        /// Create or update store procedure descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        public void CreateOrUpdateStoreProcedureDescription(string astrDatabaseName, string astrDescriptionValue, string astrSchemaName, string astrStoreProcedureName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                dbSqldocContext.CreateOrUpdateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName);
            }
        }
        /// <summary>
        /// Create or Update store procedure parameter description
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <param name="astrStoreProcedureParameterName"></param>

        public void CreateOrUpdateStoreProcParameterDescription(string astrDatabaseName, string astrDescriptionValue, string astrSchemaName, string astrStoreProcedureName, string astrStoreProcedureParameterName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                dbSqldocContext.CreateOrUpdateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName, astrStoreProcedureParameterName);
            }
        }

        /// <summary>
        /// Get store procedure descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public string GetStoreProcedureDescription(string astrDatabaseName, string astrStoreProcedureName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetStoreProcedureDescription(astrStoreProcedureName);
            }
        }
    }
}