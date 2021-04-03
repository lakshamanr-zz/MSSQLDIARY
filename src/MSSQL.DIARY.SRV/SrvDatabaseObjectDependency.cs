using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseObjectDependency :SrvMain
    {

        /// <summary>
        /// Get object details that depends
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string GetObjectThatDependsOn(string astrDatabaseConnection, string astrObjectName)
        {
            using (var dbSqlDocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return GetObjectThatDependsOnJson(dbSqlDocContext.GetObjectThatDependsOn(astrObjectName));
            }
        }

        /// <summary>
        /// Get object that depends on Json
        /// </summary>
        /// <param name="alstReferenceModels"></param>
        /// <returns></returns>
        private string GetObjectThatDependsOnJson(List<ReferencesModel> alstReferenceModels)
        {
            HierarchyJsonGenerator lHierarchyJsonGenerator = new HierarchyJsonGenerator(AddObjectTypeInfo(alstReferenceModels).Select(x => x.ThePath.Replace("\\", " ")).ToList(), "That Depends On"); 
            return  lHierarchyJsonGenerator.root.PrimengToJson();
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string GetObjectOnWhichDepends(string astrDatabaseConnection, string astrObjectName)
        {
            using (var dbSqlDocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return GetObjectOnWhichDependsOnJson(dbSqlDocContext.GetObjectOnWhichDepends(astrObjectName));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alstReferenceModels"></param>
        /// <returns></returns>
        private string GetObjectOnWhichDependsOnJson(List<ReferencesModel> alstReferenceModels)
        {
            HierarchyJsonGenerator lHierarchyJsonGenerator = new HierarchyJsonGenerator(AddObjectTypeInfo(alstReferenceModels).Select(x => x.ThePath.Replace("\\", " ")).ToList(), "On Which Depends");
            return lHierarchyJsonGenerator.root.PrimengToJson(); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referencesModels"></param>
        /// <returns></returns>
        private List<ReferencesModel> AddObjectTypeInfo(List<ReferencesModel> referencesModels)
        {
            referencesModels.DistinctBy(x => x.ThePath).ForEach(x =>
            {
                switch (x.TheType.Trim())
                {
                    case "AF ":
                    {
                        x.ThePath += "(Aggregate function)";
                    }
                        break;
                    case "C":
                    {
                        x.ThePath += "(CHECK constraint)";
                    }
                        break;
                    case "D":
                    {
                        x.ThePath += "( DEFAULT )";
                    }
                        break;
                    case "FN":
                    {
                        x.ThePath += "( SQL scalar function )";
                    }
                        break;
                    case "FS":
                    {
                        x.ThePath += "( Assembly (CLR) scalar-function )";
                    }
                        break;
                    case "FT":
                    {
                        x.ThePath += "( Assembly (CLR) table-valued function )";
                    }
                        break;
                    case "IF":
                    {
                        x.ThePath += "( SQL inline table-valued function )";
                    }
                        break;
                    case "IT":
                    {
                        x.ThePath += "( Internal table )";
                    }
                        break;
                    case "P":
                    {
                        x.ThePath += "( SQL Stored Procedure)";
                    }
                        break;
                    case "PC":
                    {
                        x.ThePath += "( Assembly (CLR) stored-procedure )";
                    }
                        break;
                    case "PG":
                    {
                        x.ThePath += "(Plan guide)";
                    }
                        break;
                    case "PK":
                    {
                        x.ThePath += "(PRIMARY KEY constraint)";
                    }
                        break;
                    case "R":
                    {
                        x.ThePath += "(Rule (old-style, stand-alone))";
                    }
                        break;
                    case "RF":
                    {
                        x.ThePath += "(Replication-filter-procedure)";
                    }
                        break;
                    case "S":
                    {
                        x.ThePath += "(System base table)";
                    }
                        break;
                    case "SN":
                    {
                        x.ThePath += "(Synonym)";
                    }
                        break;
                    case "SO":
                    {
                        x.ThePath += "(Sequence object)";
                    }
                        break;
                    case "U":
                    {
                        x.ThePath += "( Table- user-defined)";
                    }
                        break;
                    case "V":
                    {
                        x.ThePath += "(View)";
                    }
                        break;
                    case "EC":
                    {
                        x.ThePath += "(Edge constraint)";
                    }
                        break;
                    case "SQ":
                    {
                        x.ThePath += "(Service queue)";
                    }
                        break;
                    case "TA":
                    {
                        x.ThePath += "( Assembly (CLR) DML trigger)";
                    }
                        break;
                    case "TF":
                    {
                        x.ThePath += "(SQL table-valued-function)";
                    }
                        break;
                    case "TR":
                    {
                        x.ThePath += "( SQL DML trigger)";
                    }
                        break;
                    case "TT":
                    {
                        x.ThePath += "( Table type)";
                    }
                        break;
                    case "UQ":
                    {
                        x.ThePath += "( UNIQUE constraint)";
                    }
                        break;
                    case "X":
                    {
                        x.ThePath += "( Extended stored procedure)";
                    }
                        break;
                    case "XMLC":
                    {
                        x.ThePath += "(XML Data Type)";
                    }
                        break;
                    
                }
            });

            return referencesModels;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrObjectThatDependsOn"></param>
        /// <param name="astrObjectOnWhichDepends"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string GetJsonResult(string astrObjectThatDependsOn, string astrObjectOnWhichDepends, string astrObjectName)
        {
            var lstrDependencyTreeDetails = "{" +
                       "  \"data\": [" +
                       "    {" +
                       "      \"label\": \"Dependency Tree\"," +
                       "      \"expandedIcon\": \"fa fa-folder-open\"," +
                       "      \"collapsedIcon\": \"fa fa-folder-close\"," +
                       "      \"children\": " +
                       "	  [" +
                       $"{astrObjectThatDependsOn}" +
                       " ," +
                       $"{astrObjectOnWhichDepends}" +
                       "   ]" +
                       "} " +
                       "]" +
                       "}";
            return lstrDependencyTreeDetails;
        }
         
    }
}