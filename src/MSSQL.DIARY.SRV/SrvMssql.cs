using MoreLinq;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Enums;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;
using MSSQL.DIARY.ERDIAGRAM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSSQL.DIARY.SRV
{
    public class SrvMssql
    {
        public SrvMssql()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrFunctionType"></param>
        public SrvMssql(string astrFunctionType)
        {
            IstrFunctionType = astrFunctionType;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IstrDatabaseConnection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<string> ObjectExplorerDetails = new NaiveCache<string>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<List<SP_PropertyInfo>> StoreProcedureDescription = new NaiveCache<List<SP_PropertyInfo>>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<List<ExecutionPlanInfo>> CacheExecutionPlan = new NaiveCache<List<ExecutionPlanInfo>>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<string> CacheThatDependsOn = new NaiveCache<string>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<List<TablePropertyInfo>> CacheAllTableDetails = new NaiveCache<List<TablePropertyInfo>>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<List<PropertyInfo>> CacheViewDetails = new NaiveCache<List<PropertyInfo>>();
        /// <summary>
        /// 
        /// </summary>
        public static NaiveCache<List<TableFragmentationDetails>> CacheTableFragmentation = new NaiveCache<List<TableFragmentationDetails>>();


        public string IstrFunctionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static string IstrProjectName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static string IstrServerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static string IstrDatabaseName { get; set; }
        /// <summary>
        /// Get set of database object list
        /// </summary>
        /// <returns></returns>
        public List<string> GetDatabaseObjectTypes()
        {
            return new List<string>
            {
                "Tables",
                "Views",
                "Stored Procedures",
                "Table-valued Functions",
                "Scalar-valued Functions",
                "Database Triggers",
                "User-Defined Data Types",
                "XML Schema Collections",
                "Full Text Catalogs",
                "Users",
                "Database Roles",
                "Schemas"
            };
        }

        /// <summary>
        /// Get database properties
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseProperties()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetDatabaseProperties();
        }

        /// <summary>
        /// Get database option 
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseOptions()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetDatabaseOptions();
        }

        /// <summary>
        /// Get database Files
        /// </summary>
        /// <returns></returns>
        public List<FileInfomration> GetDatabaseFiles()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetDatabaseFiles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="istrPath"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="istrSchemaName"></param>
        /// <returns></returns>
        public string GetErDiagram(string istrPath, string astrDatabaseConnection, string istrSchemaName)
        {
            return File.ReadAllText(GenGraphHtmlString(istrPath + "\\" + astrDatabaseConnection + ".svg", istrSchemaName)).Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
                    
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrPath"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="istrServerName"></param>
        /// <param name="istrSchemaName"></param>
        /// <param name="alstSelectedTables"></param>
        /// <returns></returns>
        public string GetErDiagram(string astrPath, string astrDatabaseConnection, string istrServerName, string istrSchemaName, List<string> alstSelectedTables)
        {
            return File.ReadAllText(GenGraphHtmlString(astrPath + "\\" + astrDatabaseConnection + ".svg", astrDatabaseConnection, istrSchemaName, alstSelectedTables)).Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="istrPathToStoreSvg"></param>
        /// <param name="istrSchemaName"></param>
        /// <returns></returns>
        private string GenGraphHtmlString(string istrPathToStoreSvg, string istrSchemaName)
        {
            try
            {
               // astrDatabaseConnection = astrDatabaseConnection.Split('/')[0];

            }
            catch (Exception)
            {
                // ignored
            }

            File.WriteAllBytes(istrPathToStoreSvg, GetGraphHtmlString(IstrDatabaseConnection, "svg", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".pdf"), GetGraphHtmlString(IstrDatabaseConnection, "pdf", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".png"), GetGraphHtmlString(IstrDatabaseConnection, "png", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".jpg"), GetGraphHtmlString(IstrDatabaseConnection, "jpg", istrSchemaName).ToArray());

            return istrPathToStoreSvg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="istrPathToStoreSvg"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="istrSchemaName"></param>
        /// <param name="alstSelectedTables"></param>
        /// <returns></returns>
        private string GenGraphHtmlString(string istrPathToStoreSvg, string astrDatabaseConnection, string istrSchemaName, List<string> alstSelectedTables)
        {
            try
            {
                astrDatabaseConnection = astrDatabaseConnection.Split('/')[0];

            }
            catch (Exception)
            {
                // ignored
            }
            File.WriteAllBytes(istrPathToStoreSvg, GetGraphHtmlString(astrDatabaseConnection, "svg", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".pdf"), GetGraphHtmlString(astrDatabaseConnection, "pdf", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".png"), GetGraphHtmlString(astrDatabaseConnection, "png", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".jpg"), GetGraphHtmlString(astrDatabaseConnection, "jpg", istrSchemaName, alstSelectedTables).ToArray());

            return istrPathToStoreSvg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public List<FunctionDependencies> GetFunctionDependencies(string astrDatabaseConnection, string astrFunctionName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionDependencies(astrFunctionName, IstrFunctionType).DistinctBy(x => x.name).ToList();
        }

        /// <summary>
        /// Get the function properties.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public List<FunctionProperties> GetFunctionProperties(string astrDatabaseConnection, string astrFunctionName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionProperties(astrFunctionName, IstrFunctionType);
        }

        /// <summary>
        /// Get function parameters.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>

        public List<FunctionParameters> GetFunctionParameters(string astrDatabaseConnection, string astrFunctionName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionParameters(astrFunctionName, IstrFunctionType);
        }

        /// <summary>
        /// Get function create script
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public FunctionCreateScript GetFunctionCreateScript(string astrDatabaseConnection, string astrFunctionName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionCreateScript(astrFunctionName, IstrFunctionType);
        }

        /// <summary>
        /// Get functions with it descriptions
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetFunctionsWithDescription(string astrDatabaseConnection)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionsWithDescription(IstrFunctionType);
        }
        /// <summary>
        /// Get function with description
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrFunctionName"></param>
        /// <returns></returns>
        public PropertyInfo GetFunctionWithDescription(string astrDatabaseConnection, string astrFunctionName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetFunctionsWithDescription(IstrFunctionType).FirstOrDefault(x => x.istrName.Contains(astrFunctionName)) ?? new PropertyInfo { istrName = astrFunctionName, istrValue = "" };
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
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            lSqlDatabaseContext.CreateOrUpdateFunctionDescription(astrDescriptionValue, astrSchemaName, astrFunctionName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TablePropertyInfo> GetTablesDescription()
        {
            return CacheAllTableDetails.GetOrCreate(IstrDatabaseConnection, GetTablesDescriptionFromCache);
        }

        /// <summary>
        /// If the table information is not present in cache then create
        /// </summary>
        /// <returns></returns>
        private List<TablePropertyInfo> GetTablesDescriptionFromCache()
        {
            List<TablePropertyInfo> lstTableDetails;
            string lDatabaseName;
         
            using (var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection))
            {
                lstTableDetails=lSqlDatabaseContext.GetTablesDescription();
                lDatabaseName = lSqlDatabaseContext.GetCurrentDatabaseName();
            }
            var lServerName = GetServerName(IstrDatabaseConnection).FirstOrDefault();

            lstTableDetails.ForEach(x =>
            { 
                if (string.IsNullOrEmpty(x.istrValue))
                {
                    x.istrValue = "description of the " + x.istrFullName + " is missing.";
                }
                else if (x.istrName.Contains("$") && x.istrName.Contains("\\") && x.istrName.Contains("-"))
                {
                    x.istrName = string.Empty;
                }
                x.istrNevigation = lDatabaseName + "/" + x.istrFullName + "/" + lServerName;
            });

            var lstTableColumns = GetTableColumns();
            lstTableDetails.ForEach(tablePropertyInfo =>
            {
                tablePropertyInfo.tableColumns = lstTableColumns
                    .Where(x => x.tableWithSchemaname.Equals(tablePropertyInfo.istrFullName)).ToList();
            });
            return lstTableDetails;
        }

        /// <summary>
        /// Get Table descriptions
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public Ms_Description GetTableDescription(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableDescription(astrTableName);
        }

        /// <summary>
        /// Get table Indexes
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableIndexInfo> LoadTableIndexes(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableIndexes(astrTableName);
        }

        /// <summary>
        /// Get table create script
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public TableCreateScript GetTableCreateScript(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableCreateScript(astrTableName);
        }

        /// <summary>
        /// Get table dependencies
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<Tabledependencies> GetTableDependencies(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableDependencies(astrTableName);
        }
        /// <summary>
        /// Get column information of the table
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>

        public List<TableColumns> GetTableColumns(string astrTableName=null)
        { 
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            var lTableColums= lSqlDatabaseContext.GetTablesColumn(astrTableName);
            var count = 1;
            lTableColums.ForEach(lTableColumns =>
            {

                lTableColumns.id = count;
                count++;
            });
            return lTableColums;
        }

        /// <summary>
        /// Get Table foreign key details
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableForeignKeys(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableForeignKeys(astrTableName);
        }
        /// <summary>
        /// Get table Key constraints
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTableKeyConstraints(astrTableName);
        }

        /// <summary>
        /// Create or update column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnDescription"></param>
        /// <returns></returns>
        public bool CreateOrUpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnDescription)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            lSqlDatabaseContext.CreateOrUpdateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnDescription);
            return true;
        }

        /// <summary>
        /// Create or update the table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="aastrTableName"></param>
        public void CreateOrUpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string aastrTableName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            lSqlDatabaseContext.CreateOrUpdateTableDescription(astrDescriptionValue, astrSchemaName, aastrTableName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public string CreatorOrGetDependencyTree(string astrTableName)
        {
            return CacheThatDependsOn.GetOrCreate(IstrDatabaseConnection + "Table" + astrTableName, () => CreateOrGetCacheTableThatDependenceOn(astrTableName));
        }

        /// <summary>
        /// Cache table related dependency
        /// </summary>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string CreateOrGetCacheTableThatDependenceOn(string astrObjectName)
        {

            return GetJsonResult(GetObjectThatDependsOn(IstrDatabaseConnection, astrObjectName), GetObjectOnWhichDepends(IstrDatabaseConnection, astrObjectName), astrObjectName);
        }


        /// <summary>
        /// Cache table related fragmentation details
        /// </summary>
        /// <returns></returns>
        public List<TableFragmentationDetails> CacheTableFragmentationDetails()
        {
            return CacheTableFragmentation.GetOrCreate(IstrDatabaseConnection, GetTablesFragmentation);
        }
        /// <summary>
        /// Get all table fragmentation details
        /// </summary>
        /// <returns></returns>
        private List<TableFragmentationDetails> GetTablesFragmentation()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetTablesFragmentation();
        }
        /// <summary>
        /// Get Table Fragmentation for the cache
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFragmentationDetails> GetTableFragmentationDetails(string astrTableName)
        {
            return CacheTableFragmentationDetails().Where(x => x.TableName.Equals(astrTableName)).ToList();
        }
        /// <summary>
        /// Get object details that depends
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string GetObjectThatDependsOn(string astrDatabaseConnection, string astrObjectName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return GetObjectThatDependsOnJson(lSqlDatabaseContext.GetObjectThatDependsOn(astrObjectName));
        }

        /// <summary>
        /// Get object that depends on Json
        /// </summary>
        /// <param name="alstReferenceModels"></param>
        /// <returns></returns>
        private string GetObjectThatDependsOnJson(List<ReferencesModel> alstReferenceModels)
        {
            HierarchyJsonGenerator lHierarchyJsonGenerator = new HierarchyJsonGenerator(AddObjectTypeInfo(alstReferenceModels).Select(x => x.ThePath.Replace("\\", " ")).ToList(), "That Depends On");
            return lHierarchyJsonGenerator.root.PrimengToJson();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string GetObjectOnWhichDepends(string astrDatabaseConnection, string astrObjectName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return GetObjectOnWhichDependsOnJson(lSqlDatabaseContext.GetObjectOnWhichDepends(astrObjectName));
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
                if (x.TheType.IsNotNullOrEmpty())
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
        /// <summary>
        /// Get the  Graph Html string
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrFormatType"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public byte[] GetGraphHtmlString(string astrDatabaseName, string astrFormatType, string astrSchemaName)
        {
            IstrDatabaseConnection = astrDatabaseName;
            // adding sub graph 
            var lGraphSvg = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
            if (astrSchemaName.IsNullOrEmpty())
                GetTablesDescription().ForEach(x =>
                {
                    var columnDictionary = new Dictionary<string, string>();
                    x.tableColumns.ForEach(x2 =>
                    {
                        columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                    });
                    var lTableWithSchema = new TableWithSchema();
                    var ldcTablesAndColumns = new Dictionary<string, Dictionary<string, string>>
                    {
                        {x.istrFullName, columnDictionary}
                    };


                    lTableWithSchema.keyValuePairs = ldcTablesAndColumns;
                    lTableWithSchema.istrSchemaName = x.istrSchemaName;
                    lstTablesAndColumns.Add(lTableWithSchema);
                });
            else
                GetTablesDescription()
                     .Where(x => x.istrSchemaName == astrSchemaName).ForEach(x =>
                     {
                         var columnDictionary = new Dictionary<string, string>();
                         x.tableColumns.ForEach(x2 =>
                         {
                             columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                         });
                         var lTableWithSchema = new TableWithSchema();
                         var ldcTablesAndColumns = new Dictionary<string, Dictionary<string, string>>
                         {
                            {x.istrFullName, columnDictionary}
                         };


                         lTableWithSchema.keyValuePairs = ldcTablesAndColumns;
                         lTableWithSchema.istrSchemaName = x.istrSchemaName;
                         lstTablesAndColumns.Add(lTableWithSchema);
                     });

            lGraphSvg.SetListOfTables(lstTablesAndColumns, astrSchemaName);

            if (astrFormatType.Equals("pdf"))
                return FileDotEngine.Pdf(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));

            return FileDotEngine.Svg(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
        }

        /// <summary>
        /// Get Graph Html string with specific schemaName
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrFormatType"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="alstOfSelectedTables"></param>
        /// <returns></returns>
        public byte[] GetGraphHtmlString(string astrDatabaseName, string astrFormatType, string astrSchemaName, List<string> alstOfSelectedTables)
        {
            var graphHtml = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
            var lstTablePropertyInfo = new List<TablePropertyInfo>();
            IstrDatabaseConnection = astrDatabaseName;
            if (astrSchemaName.IsNullOrEmpty())
            {
                GetTablesDescription().ForEach(x =>
                 {
                     if (alstOfSelectedTables.Any(argtbl => argtbl.Equals(x.istrName)))
                     {
                         lstTablePropertyInfo.Add(x);
                     }
                 });
                lstTablePropertyInfo.ForEach(x =>
                {
                    SelectTableWithOutSchemaNames(x, lstTablesAndColumns);
                });
            }
            else
            {
                GetTablesDescription().ForEach(x =>
                {
                    if (alstOfSelectedTables.Any(argtbl => argtbl.Equals(x.istrName)))
                    {
                        lstTablePropertyInfo.Add(x);
                    }
                });

                lstTablePropertyInfo
                     .Where(x => x.istrSchemaName == astrSchemaName)
                     .ForEach(x =>
                     {
                         SelectTableWithSchemaNames(x, lstTablesAndColumns);
                     });
            }

            graphHtml.SetListOfTables(lstTablesAndColumns, astrSchemaName);

            if (astrFormatType.Equals("pdf"))
                return FileDotEngine.Pdf(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));

            return FileDotEngine.Svg(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
        }

        /// <summary>
        /// Select Table with specified schema
        /// </summary>
        /// <param name="aTablePropertyInfo"></param>
        /// <param name="lstTablesAndColumns"></param>
        private static void SelectTableWithSchemaNames(TablePropertyInfo aTablePropertyInfo, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            aTablePropertyInfo.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var lTableWithSchema = new TableWithSchema();
            var lTablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            lTablesAndColumns.Add(aTablePropertyInfo.istrFullName, columnDictionary);


            lTableWithSchema.keyValuePairs = lTablesAndColumns;
            lTableWithSchema.istrSchemaName = aTablePropertyInfo.istrSchemaName;
            lstTablesAndColumns.Add(lTableWithSchema);
        }

        /// <summary>
        /// Select Table with out schema
        /// </summary>
        /// <param name="aTablePropertyInfo"></param>
        /// <param name="lstTablesAndColumns"></param>
        private static void SelectTableWithOutSchemaNames(TablePropertyInfo aTablePropertyInfo, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            aTablePropertyInfo.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var tableWithSchema = new TableWithSchema();
            var tablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            tablesAndColumns.Add(aTablePropertyInfo.istrFullName, columnDictionary);


            tableWithSchema.keyValuePairs = tablesAndColumns;
            tableWithSchema.istrSchemaName = aTablePropertyInfo.istrSchemaName;
            lstTablesAndColumns.Add(tableWithSchema);
        }
        /// <summary>
        /// Get list of schemas and there description
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetSchemaWithDescriptions(string astrDatabaseConnection)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetSchemaWithDescriptions();
        }

        /// <summary>
        /// Create or update the schema descriptions
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        public void CreateOrUpdateSchemaMsDescription(string astrDatabaseConnection, string astrDescriptionValue, string astrSchemaName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            lSqlDatabaseContext.CreateOrUpdateSchemaDescription(astrDescriptionValue, astrSchemaName);
        }

        /// <summary>
        /// Get schema references with table / view / store procedures etc. 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<SchemaReferanceInfo> GetSchemaReferences(string astrDatabaseConnection, string astrSchemaName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetSchemaReferences(astrSchemaName);
        }

        /// <summary>
        /// Get the schema description.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public Ms_Description GetSchemaDescription(string astrDatabaseConnection, string astrSchemaName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetSchemaDescription(astrSchemaName);
        }
        /// <summary>
        /// Get server names
        /// </summary>
        /// <returns></returns>
        public List<string> GetServerName(string astrDatabaseConnection=null)
        {
            var lstServers = new List<string>();
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            lstServers.Add(lSqlDatabaseContext.GetServerName().SERVERNAME);
            return lstServers;
        }
        /// <summary>
        /// Get database names.
        /// </summary>
        /// <returns></returns>
        public List<DatabaseName> GetDatabaseNames()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetDatabaseNames();
        }
        /// <summary>
        /// Get server Properties
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetServerProperties()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetServerProperties();
        }
        /// <summary>
        /// Get server advance properties.
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetAdvancedServerSettings()
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(IstrDatabaseConnection);
            return lSqlDatabaseContext.GetAdvancedServerSettings();
        }

        /// <summary>
        /// Get store procedure from cache
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public List<SP_PropertyInfo> GetStoreProceduresWithDescription(string astrDatabaseName)
        {
            return StoreProcedureDescription.GetOrCreate(astrDatabaseName + "GetStoreProceduresWithDescription", () => GetStoreProcedureFromCache(astrDatabaseName));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        private List<SP_PropertyInfo> GetStoreProcedureFromCache(string astrDatabaseName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            var lstStoreProcedureDetails = new List<SP_PropertyInfo>();
            foreach (var storeProcedureKeyValuePair in lSqlDatabaseContext.GetStoreProceduresWithDescription().GroupBy(x => x.istrName))
            {
                var lstrStoreProcedureDetails = new SP_PropertyInfo { istrName = storeProcedureKeyValuePair.Key };

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

        /// <summary>
        /// Get create script of the store procedure
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public Ms_Description GetStoreProcedureCreateScript(string astrDatabaseName, string astrStoreProcedureName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetStoreProcedureCreateScript(astrStoreProcedureName);
        }

        /// <summary>
        /// Get store procedure dependencies
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<SP_Depencancy> GetStoreProceduresDependency(string astrDatabaseName, string astrStoreProcedureName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetStoreProceduresDependency(astrStoreProcedureName);
        }

        /// <summary>
        /// Store the store procedure dependency tree and if already present then load 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public string CreatorOrGetStoreProcedureDependencyTree(string astrDatabaseName, string astrStoreProcedureName)
        {
            return CacheThatDependsOn.GetOrCreate(astrDatabaseName + "StoreProcedure" + astrStoreProcedureName, () => CreateOrGetCacheStoreProcedureThatDependsOn(astrDatabaseName, astrStoreProcedureName));
        }

        /// <summary>
        /// Get store procedure that dependent on
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// 
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string CreateOrGetCacheStoreProcedureThatDependsOn(string astrDatabaseName, string astrObjectName)
        {
            return GetJsonResult(GetObjectOnWhichDepends(astrDatabaseName, astrObjectName), GetObjectThatDependsOn(astrDatabaseName, astrObjectName), astrObjectName);
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
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            foreach (var storeProceduresParameterKey in lSqlDatabaseContext.GetStoreProceduresParametersWithDescription(astrStoreProcedureName).GroupBy(x => x.Parameter_name))
            {
                var lStoreProcedureParameter = new SP_Parameters { Parameter_name = storeProceduresParameterKey.Key };
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

            return lstStoreProcedureParameters;
        }

        /// <summary>
        /// Get store procedure execution plan details
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public List<ExecutionPlanInfo> GetStoreProcedureExecutionPlan(string astrDatabaseName, string astrStoreProcedureName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            var lstExecutionPlanDetails = lSqlDatabaseContext.GetStoreProcedureExecutionPlan(astrStoreProcedureName);

            if (lstExecutionPlanDetails.Any())
            {
                return CacheExecutionPlan.GetOrCreate(astrDatabaseName + astrStoreProcedureName, () => lstExecutionPlanDetails);
            }

            if (CacheExecutionPlan.Cache.TryGetValue(astrDatabaseName + astrStoreProcedureName, out var executionPlanDetails))
                return executionPlanDetails;
            return lstExecutionPlanDetails;
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
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            lSqlDatabaseContext.CreateOrUpdateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName);
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
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            lSqlDatabaseContext.CreateOrUpdateStoreProcedureDescription(astrDescriptionValue, astrSchemaName, astrStoreProcedureName, astrStoreProcedureParameterName);
        }

        /// <summary>
        /// Get store procedure descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrStoreProcedureName"></param>
        /// <returns></returns>
        public Ms_Description GetStoreProcedureDescription(string astrDatabaseName, string astrStoreProcedureName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetStoreProcedureDescription(astrStoreProcedureName);
        }

        /// <summary>
        /// Get Database Triggers
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetTriggers(string astrDatabaseName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetTriggers();
        }

        /// <summary>
        /// Get Trigger Details by trigger name
        /// </summary>
        /// <returns></returns>
        public List<TriggerInfo> GetTrigger(string astrDatabaseName, string istrTriggerName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetTrigger(istrTriggerName);
        }

        /// <summary>
        /// Create or update the trigger descriptions
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrTriggerName"></param>
        public void CreateOrUpdateTriggerDescription(string astrDatabaseName, string astrDescriptionValue, string astrTriggerName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            lSqlDatabaseContext.CreateOrUpdateTriggerDescription(astrDescriptionValue, astrTriggerName);
        }
        /// <summary>
        /// Get details about all used defined data types
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public List<UserDefinedDataTypeDetails> GetUserDefinedDataTypes(string astrDatabaseName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetUserDefinedDataTypes();
        }
        /// <summary>
        /// Get details about specific user defined data type
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="istrTypeName"></param>
        /// <returns></returns>

        public UserDefinedDataTypeDetails GetUserDefinedDataType(string astrDatabaseName, string istrTypeName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetUserDefinedDataType(istrTypeName);
        }
        /// <summary>
        /// Get user defined data type references
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>

        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReference(string astrDatabaseName, string astrTypeName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetUsedDefinedDataTypeReference(astrTypeName);
        }
        /// <summary>
        /// Get user defined data type extended properties
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="istrTypeName"></param>
        /// <returns></returns>

        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string istrTypeName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetUsedDefinedDataTypeExtendedProperties(istrTypeName);
        }
        /// <summary>
        /// Create or update user defined function
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescriptionValue"></param>
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string astrTypeName, string astrDescriptionValue)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            lSqlDatabaseContext.CreateOrUpdateUsedDefinedDataTypeExtendedProperties(astrTypeName, astrDescriptionValue);
        }
        /// <summary>
        /// Get Views Details with it Description
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetViewsWithDescription(string astrDatabaseName)
        {
            return CacheViewDetails.GetOrCreate(astrDatabaseName + "ViewsWithDescription", () => CacheViewsWithDescription(astrDatabaseName));
        }
        /// <summary>
        /// Cache view details 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        private static List<PropertyInfo> CacheViewsWithDescription(string astrDatabaseName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewsWithDescription();
        }
        /// <summary>
        /// Get View Dependencies
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewDependancy> GetViewDependencies(string astrDatabaseName, string astrViewName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewDependencies(astrViewName);
        }
        /// <summary>
        /// Get view Properties
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<View_Properties> GetViewProperties(string astrDatabaseName, string astrViewName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewProperties(astrViewName);
        }
        /// <summary>
        /// Get view Columns 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public List<ViewColumns> GetViewColumns(string astrDatabaseName, string astrViewName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewColumns(astrViewName);
        }
        /// <summary>
        /// Get view Create script
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>
        public ViewCreateScript GetViewCreateScript(string astrDatabaseName, string astrViewName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewCreateScript(astrViewName);
        }
        /// <summary>
        /// Get view Detail
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrViewName"></param>
        /// <returns></returns>

        public PropertyInfo GetViewsWithDescription(string astrDatabaseName, string astrViewName)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseName);
            return lSqlDatabaseContext.GetViewsWithDescription().Find(x => x.istrName.Contains(astrViewName));
        }
        //private TreeViewJson SearchInDb()
        //{
        //    return new TreeViewJson
        //    {
        //        text = "Search",
        //        icon = "fa fa-search",
        //        mdaIcon = "Search",
        //        link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables",
        //        selected = true,
        //        badge = 12,
        //        expand = true,
        //        //SchemaEnums = SchemaEnums.AllTable,
        //        children = SearchInDbObjects()
        //    };
        //}

        //private List<TreeViewJson> SearchInDbObjects()
        //{
        //    var searchInDbObject = new List<TreeViewJson>
        //    {
        //        new TreeViewJson
        //        {
        //            text = "Search In column",
        //            icon = "fa fa-search",
        //            mdaIcon = "Search In column",
        //            expand = false,
        //            link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Search/Column",
        //            selected = true,
        //            badge = 12
        //        },
        //        new TreeViewJson
        //        {
        //            text = "Search In column",
        //            icon = "fa fa-search",
        //            mdaIcon = "Search In column",
        //            expand = false,
        //            link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Search/Column",
        //            selected = true,
        //            badge = 12
        //        }
        //    };
        //    return searchInDbObject;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetDatabase(DatabaseName astrDatabaseName, string astrDatabaseConnection =null)
        {
            return new TreeViewJson
            {
                text = astrDatabaseName.databaseName,
                icon = "fa fa-database fa-fw",
                mdaIcon = astrDatabaseName.databaseName,
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllDatabase,
                children = new List<TreeViewJson>
                {
                    // SearchInDb(),
                    GetTables(astrDatabaseConnection),
                    GetViews(astrDatabaseConnection),
                    GetProgrammability(astrDatabaseConnection)
                    //GetStorage(),
                    //GetSecurity()
                }
            };
        }
         

        public static TreeViewJson GetProjectName(string astrProjectName = null, string astrServerName = null, string astrdatabaseName = null, List<DatabaseName> astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            IstrDatabaseName = astrdatabaseName;
            IstrProjectName = astrProjectName;
            return AddDbInformation(astrProjectName, astrServerName, astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrProjectName"></param>
        /// <param name="astrServerName"></param>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson AddDbInformation(string astrProjectName, string astrServerName, List<DatabaseName> astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            var leftTreeJson = new TreeViewJson
            {
                text = astrProjectName,
                icon = "fa fa-home fa-fw",
                mdaIcon = astrProjectName,
                link = $"/{IstrProjectName}/{IstrProjectName}",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.ProjectInfo,
                children = new List<TreeViewJson> { GetServerName(astrServerName, astrDatabaseNames, astrDatabaseConnection) }
            };
            return leftTreeJson;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrServerName"></param>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetServerName(string astrServerName , List<DatabaseName> astrDatabaseNames , string astrDatabaseConnection )
        {
            var result = new TreeViewJson
            {
                text = astrServerName,
                icon = "fa  fa-desktop fa-fw",
                mdaIcon = astrServerName,
                link = $"/{IstrProjectName}/{astrServerName}",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.DatabaseServer,
                children = new List<TreeViewJson> { GetDatabases(astrDatabaseNames, astrDatabaseConnection) }
            };


            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetDatabases(List<DatabaseName> astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            var rest = new TreeViewJson
            {
                text = "User Database",
                icon = "fa fa-folder",
                mdaIcon = "User Database",
                link = $"/{IstrProjectName}/{IstrServerName}/User Database",
                selected = true,
                expand = true,
                badge = 12,
                children = new List<TreeViewJson>()
            };
            astrDatabaseNames?.ForEach(dbInstance =>
            {
                IstrDatabaseName = dbInstance.databaseName;
                var databaseConnection = string.Empty;
                databaseConnection += astrDatabaseConnection?.Split(';')[0] + ";";
                databaseConnection += $"Database={IstrDatabaseName};";
                databaseConnection += astrDatabaseConnection?.Split(';')[2] + ";";
                databaseConnection += astrDatabaseConnection?.Split(';')[3] + ";";
                databaseConnection += "Trusted_Connection=false;";
                rest.children.Add(GetDatabase(dbInstance, databaseConnection));
            });
            return rest;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetObjectExplorer(string astrDatabaseConnection, string astrDatabaseName = null)
        {
            var lstDatabaseNames = new List<DatabaseName>();
            string lstrDefaultDbName;
            if (astrDatabaseName.IsNotNullOrEmpty())
            {
                lstrDefaultDbName = astrDatabaseName;
                lstDatabaseNames.Add(new DatabaseName() {databaseName = astrDatabaseName } );
            }
            else
            {
                lstrDefaultDbName = GetDatabaseName(astrDatabaseConnection).Select(x => x.databaseName).First();
                lstDatabaseNames = GetDatabaseName(astrDatabaseConnection);
            }

            var data = new List<TreeViewJson>
            {
                GetProjectName("Project", astrDatabaseConnection?.Split(';')[0].Replace("Data Source =", "").Replace("Data Source=", "") , lstrDefaultDbName, lstDatabaseNames, astrDatabaseConnection)};
            return data;
        }

        #region Tables
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetTables(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Tables",
                icon = "fa fa-folder",
                mdaIcon = "Tables",
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllTable,
                children = GetTablesChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetTablesChildren(string astrDatabaseConnection = null)
        {
            var tablesList = new List<TreeViewJson>();
            GetTables(IstrDatabaseName, astrDatabaseConnection).ForEach(tables =>
            {
                tablesList.Add(
                    new TreeViewJson
                    {
                        text = tables.istrFullName,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = tables.istrFullName,
                        expand = false,
                        link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables/{tables}",
                        selected = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Table,
                        // children = GetTableColumns(tables, astrDatabaseConnection)
                    }
                );
            });
            return tablesList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static IList<TreeViewJson> GetTableColumns(string tables, string astrDatabaseConnection )
        {
            var tablesColumns = new List<TreeViewJson>();
            GetTablesColumns(tables.Replace(tables.Substring(0, tables.IndexOf(".", StringComparison.Ordinal)) + ".", ""), IstrDatabaseName, astrDatabaseConnection).
                ForEach(columns =>
                {
                    tablesColumns.Add(
                        new TreeViewJson
                        {
                            text = columns.columnname,
                            icon = "fa fa fa-columns",
                            mdaIcon = columns.columnname,
                            expand = false,
                            link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables/{columns}",
                            selected = true,
                            badge = 12,
                            SchemaEnums = SchemaEnums.TableCoumns
                        }
                    );
                });
            return tablesColumns;
        }

        #endregion

        #region Views
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetViews(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Views",
                icon = "fa fa-folder",
                mdaIcon = "Views",
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Views",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllViews,
                children = GetViewsChildrens(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetViewsChildrens(string astrDatabaseConnection = null)
        {
            var lstViewChildren = new List<TreeViewJson>();
            GetViews(IstrDatabaseName, astrDatabaseConnection).ForEach(view =>
            {
                lstViewChildren.Add(
                    new TreeViewJson
                    {
                        text = view,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = view,
                        expand = true,
                        link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Views/{view}",
                        selected = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Views
                    }
                );
            });
            return lstViewChildren;
        }

        #endregion

        #region Programmability
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetProgrammability(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Programmability",
                icon = "fa fa-folder",
                mdaIcon = "Programmability",
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllProgrammability,
                children = new List<TreeViewJson>
                {
                    GetStoredProcedures(astrDatabaseConnection),
                    GetFunction(astrDatabaseConnection),
                    GetDatabaseTrigger(astrDatabaseConnection),
                    GetDataBaseDataTypes(astrDatabaseConnection)
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetStoredProcedures(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "StoredProcedures",
                icon = "fa fa-folder",
                mdaIcon = "StoredProcedures",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/StoredProcedures",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllStoreprocedure,
                children = GetStoredProceduresChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetStoredProceduresChildren(string astrDatabaseConnection = null)
        {
            var storeProcedureList = new List<TreeViewJson>();
            GetStoreProcedures(IstrDatabaseName, astrDatabaseConnection).ForEach(storeProcedure =>
            {
                storeProcedureList.Add(new TreeViewJson
                {
                    text = storeProcedure,
                    icon = "fa fa-table fa-fw",
                    mdaIcon = storeProcedure,
                    link =
                        $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/StoredProcedures/{storeProcedure}",
                    selected = true,
                    badge = 12,
                    SchemaEnums = SchemaEnums.Storeprocedure
                });
            });

            return storeProcedureList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetFunction(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Functions",
                icon = "fa fa-folder",
                mdaIcon = "Functions",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllFunctions,
                children = new List<TreeViewJson>
                {
                    GetTableValuedFunctions(astrDatabaseConnection),
                    GetScalarValuedFunctions(astrDatabaseConnection),
                    GetAggregateFunctions(astrDatabaseConnection)
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetTableValuedFunctions(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Table-valued Functions",
                icon = "fa fa-folder",
                mdaIcon = "Table-valued Functions",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/TableValuedFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllTableValueFunction,
                children = GetTableValuedFunctionsChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetTableValuedFunctionsChildren(string astrDatabaseConnection = null)
        {
            var tableValuedFunctions = new List<TreeViewJson>();
            GetTableValueFunctions(IstrDatabaseName, astrDatabaseConnection).ForEach(lDatabaseFunctions =>
            {
                tableValuedFunctions.Add(
                    new TreeViewJson
                    {
                        text = lDatabaseFunctions,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = lDatabaseFunctions,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/TableValuedFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.TableValueFunction
                    }
                );
            });
            return tableValuedFunctions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetScalarValuedFunctions(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Scalar-valued Functions",
                icon = "fa fa-folder",
                mdaIcon = "Scalar-valued Functions",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/ScalarValuedFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllScalarValueFunctions,
                children = GetScalarValuedFunctionsChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetScalarValuedFunctionsChildren(string astrDatabaseConnection = null)
        {
            var lstScalarFunctions = new List<TreeViewJson>();

            GetScalarFunctions(IstrDatabaseName, astrDatabaseConnection).ForEach(lDatabaseFunctions =>
            {
                lstScalarFunctions.Add(
                    new TreeViewJson
                    {
                        text = lDatabaseFunctions,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = lDatabaseFunctions,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/ScalarValuedFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.ScalarValueFunctions
                    }
                );
            });
            return lstScalarFunctions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetAggregateFunctions(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Aggregate Functions",
                icon = "fa fa-folder",
                mdaIcon = "Aggregate Functions",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/AggregateFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllAggregateFunciton,
                children = GetAggregateFunctionsChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetAggregateFunctionsChildren(string astrDatabaseConnection = null)
        {
            var lstAggregateFunctions = new List<TreeViewJson>();
            GetAggregateFunctions(IstrDatabaseName, astrDatabaseConnection).ForEach(lDatabaseFunctions =>
            {
                lstAggregateFunctions.Add(
                    new TreeViewJson
                    {
                        text = lDatabaseFunctions,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = lDatabaseFunctions,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/AggregateFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.AggregateFunciton
                    }
                );
            });
            return lstAggregateFunctions;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetDatabaseTrigger(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "DatabaseTrigger",
                icon = "fa fa-folder",
                mdaIcon = "DatabaseTrigger",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DatabaseTrigger",
                selected = true,
                expand = true,
                SchemaEnums = SchemaEnums.AllTriggers,
                badge = 12,
                children = GetDatabaseTriggerChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetDatabaseTriggerChildren(string astrDatabaseConnection = null)
        {
            var databaseTrigger = new List<TreeViewJson>();
            GetTriggers(IstrDatabaseName, astrDatabaseConnection).ForEach(lDatabaseTrigger =>
            {
                databaseTrigger.Add(
                    new TreeViewJson
                    {
                        text = lDatabaseTrigger,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = lDatabaseTrigger,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DatabaseTrigger/{lDatabaseTrigger}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Triggers
                    }
                );
            });
            return databaseTrigger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetDataBaseDataTypes(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "Type",
                icon = "fa fa-folder",
                mdaIcon = "Type",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllDatabaseDataTypes,
                children = new List<TreeViewJson>
                {
                    GetUserDefinedDataType(astrDatabaseConnection),
                    GetXmlSchemas(astrDatabaseConnection)
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetUserDefinedDataType(string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = "User-Defined Data Types",
                icon = "fa fa-folder",
                mdaIcon = "User Defined Data Types",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/UserDefinedDataType",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllUserDefinedDataType,
                children = GetUserDefinedDataTypeChildren(astrDatabaseConnection)
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetUserDefinedDataTypeChildren(string astrDatabaseConnection = null)
        {
            var userDefinedType = new List<TreeViewJson>();
            GetUserDefinedType(IstrDatabaseName, astrDatabaseConnection).ForEach(lUserDefinedType =>
            {
                userDefinedType.Add(
                    new TreeViewJson
                    {
                        text = lUserDefinedType,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = lUserDefinedType,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/UserDefinedDataType/{lUserDefinedType}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.UserDefinedDataType
                    }
                );
            });
            return userDefinedType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static TreeViewJson GetXmlSchemas(string astrDatabaseConnection = null)
        {
            return new TreeViewJson {text = "XML Schema Collections", icon = "fa fa-folder", mdaIcon = "XML Schema Collections", link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection", selected = true, expand = true, badge = 12, SchemaEnums = SchemaEnums.AllXMLSchemaCollection, children = GetXmlSchemasChildren(astrDatabaseConnection)};
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TreeViewJson> GetXmlSchemasChildren(string astrDatabaseConnection = null)
        {
            var definedType = new List<TreeViewJson>();
            GetTriggers(IstrDatabaseName, astrDatabaseConnection).ForEach(lXmlSchema =>
            {
                definedType.Add(new TreeViewJson {text = lXmlSchema, mdaIcon = lXmlSchema, link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection/{lXmlSchema}", selected = true, expand = true, badge = 12, SchemaEnums = SchemaEnums.XMLSchemaCollection});
            });
            return definedType;
        }

        #endregion 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<DatabaseName> GetDatabaseName(string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetDatabaseNames();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TablePropertyInfo> GetTables(string astrDatabaseName, string astrDatabaseConnection)
        {
            return GetTableList(astrDatabaseName, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TablePropertyInfo> GetTableList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetTablesDescription();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TableColumns> GetTablesColumns(string astrTableName, string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetTablesColumnsList(astrTableName, astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<TableColumns> GetTablesColumnsList(string astrTableName, string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetTableColumns(astrTableName);
        }

        //GetTableColumns
        //public static List<string> GetTableColumns(string astrColumnName)
        //{
        //    using (var lSqlDatabaseContext = new MsSqlDiaryContext())
        //    {
        //        return lSqlDatabaseContext.GetTableColumns(astrColumnName);
        //    }
        //}

        public static List<string> GetViews(string astrDatabaseNames ,string astrDatabaseConnection)
        {
            return GetViewsList(astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetViewsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetViewsWithDescription().Select(x => x.istrName).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetStoreProcedures(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetStoreProceduresList(astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetStoreProceduresList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetStoreProceduresWithDescription().Where(x => x != null).Select((x=>x.istrName)).ToList();
        }

        public static List<string> GetScalarFunctions(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetScalarFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        } 
        public static List<string> GetScalarFunctionsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetScalarFunctions().Where(x => x != null).Select(x=>x.SQL_SCALAR_FUNCTION).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetTableValueFunctions(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetTableValueFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetTableValueFunctionsList(string astrDatabaseNames, string astrDatabaseConnection)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetTableValueFunctions().Where(x => x != null).Select(x=>x.SQL_TABLE_VALUED_FUNCTION).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetAggregateFunctions(string astrDatabaseNames, string astrDatabaseConnection)
        {
            return GetAggregateFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetAggregateFunctionsList(string astrDatabaseNames, string astrDatabaseConnection )
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetAggregateFunctions().Where(x => x != null).Select(x=>x.SQL_AGGREGATE_FUNCTION).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetTriggers(string astrDatabaseNames, string astrDatabaseConnection)
        {
            return GetTriggersList(astrDatabaseNames, astrDatabaseConnection);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetTriggersList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetTriggers().Where(x => x.istrName != null).Select(x => x.istrName).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="astrDatabaseNames"></param>
        /// <returns></returns>
        public static List<string> GetUserDefinedTypesList(string astrDatabaseNames = null)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseNames);
            return lSqlDatabaseContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x => x.name).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="istrDatabaseName"></param>
        /// <param name="astrDatabaseConnection"></param>
        /// <returns></returns>
        public static List<string> GetUserDefinedType(string istrDatabaseName, string astrDatabaseConnection)
        {
            using var lSqlDatabaseContext = new MsSqlDiaryContext(astrDatabaseConnection);
            return lSqlDatabaseContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x => x.name).ToList();
        }

    }
}
