using System.Collections.Generic;
using System.Linq;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Enums;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvObjectExplorerDetails
    {
        public static NaiveCache<string> ObjectExplorerDetails = new NaiveCache<string>();
        public SrvDatabaseObjectDependency SrvDatabaseObjectDependency; 
        public SrvObjectExplorerDetails()
        {
            ObjectExplorerDetails = new NaiveCache<string>();
            SrvDatabaseObjectDependency = new SrvDatabaseObjectDependency();
        }
        public static string IstrProjectName { get; set; }
        public static string astrServerName { get; set; }
        public static string IstrDatabaseName { get; set; }

        private TreeViewJson SearchInDb()
        {
            return new TreeViewJson
            {
                text = "Search",
                icon = "fa fa-search",
                mdaIcon = "Search",
                link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Tables",
                selected = true,
                badge = 12,
                expand = true,
                //SchemaEnums = SchemaEnums.AllTable,
                children = SearchInDbObjects()
            };
        }

        private List<TreeViewJson> SearchInDbObjects()
        {
            var searchInDbObject = new List<TreeViewJson>
            {
                new TreeViewJson
                {
                    text = "Search In column",
                    icon = "fa fa-search",
                    mdaIcon = "Search In column",
                    expand = false,
                    link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Search/Column",
                    selected = true,
                    badge = 12
                    //SchemaEnums = SchemaEnums.TableCoumns,
                },
                new TreeViewJson
                {
                    text = "Search In column",
                    icon = "fa fa-search",
                    mdaIcon = "Search In column",
                    expand = false,
                    link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Search/Column",
                    selected = true,
                    badge = 12
                    //SchemaEnums = SchemaEnums.TableCoumns,
                }
            };
            return searchInDbObject;
        }

        public static TreeViewJson GetDatabase(string astrdatabaseName = null, string astrDatabaseConnection = null)
        {
            return new TreeViewJson
            {
                text = astrdatabaseName,
                icon = "fa fa-database fa-fw",
                mdaIcon = astrdatabaseName,
                link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}",
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

        public static TreeViewJson GetUserDatabaseNames(string astrUserDataBaseName = null, string astrdatabaseName = null)
        {
            return new TreeViewJson
            {
                text = astrUserDataBaseName,
                //icon = "fa fa-home fa-fw",
                mdaIcon = astrUserDataBaseName,
                link = "/home/dashboard",
                selected = true,
                badge = 12,
                children = new List<TreeViewJson>
                {
                    GetDatabase(astrdatabaseName)
                }
            };
        }

        public static TreeViewJson GetProjectName(string astrProjectName = null, string astrServerName = null, string astrdatabaseName = null, List<string> astrDatabaseNames = null, string astrDatabaseConnection=null)
        {
            astrServerName = astrServerName;
            IstrDatabaseName = astrdatabaseName;
            IstrProjectName = astrProjectName;
            return AddDbInformations(astrProjectName, astrServerName, astrDatabaseNames, astrDatabaseConnection);
        }

        public static TreeViewJson AddDbInformations(string astrProjectName, string astrServerName,  List<string> astrDatabaseNames = null, string astrDatabaseConnection=null)
        {
            var leftTreeJoson = new TreeViewJson
            {
                text = astrProjectName,
                icon = "fa fa-home fa-fw",
                mdaIcon = astrProjectName,
                link = $"/{IstrProjectName}/{IstrProjectName}",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.ProjectInfo,
                children = new List<TreeViewJson> {GetServerName(astrServerName, astrDatabaseNames, astrDatabaseConnection) }
            }; 
            return leftTreeJoson;
        }

        public static TreeViewJson GetServerName(string astrServerName = null, List<string> astrDatabaseNames = null, string astrDatabaseConnection= null)
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
                children = new List<TreeViewJson> {GetDatabases(astrDatabaseNames, astrDatabaseConnection) }
            };


            return result;
        }

        public static TreeViewJson GetDatabases(List<string> astrDatabaseNames = null, string astrDatabaseConnection= null)
        {
            var rest = new TreeViewJson
            {
                text = "User Database",
                icon = "fa fa-folder",
                mdaIcon = "User Database",
                link = $"/{IstrProjectName}/{astrServerName}/User Database",
                selected = true,
                expand = true,
                badge = 12,
                children = new List<TreeViewJson>()
            };
            astrDatabaseNames?.ForEach(dbInstance =>
            {
                IstrDatabaseName = dbInstance;
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
        public static List<TreeViewJson> GetObjectExplorer(string astrDatabaseConnection,string astrDatabaseName=null)
        {
            var lstDatabase = new List<string>();
            string lstrDefaultDbName;
            if (astrDatabaseName.IsNotNullOrEmpty())
            {
                lstrDefaultDbName = astrDatabaseName;
                lstDatabase.Add(astrDatabaseName);
            }
            else
            {
                lstrDefaultDbName = GetDatabaseName(astrDatabaseConnection).FirstOrDefault(); 
                 lstDatabase = GetDatabaseName(astrDatabaseConnection);
            }

            var data = new List<TreeViewJson>
            {
                GetProjectName("Project", astrDatabaseConnection?.Split(';')[0].Replace("Data Source =", "").Replace("Data Source=", "") , lstrDefaultDbName, lstDatabase, astrDatabaseConnection)};
            return data;
        }

        #region Tables

        public static TreeViewJson GetTables(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "Tables",
                icon = "fa fa-folder",
                mdaIcon = "Tables",
                link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Tables",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllTable,
                children = GetTablesChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetTablesChildren(string astrDatabaseConnection=null)
        {
            var tablesList = new List<TreeViewJson>();
            GetTables(IstrDatabaseName, astrDatabaseConnection).ForEach(tables =>
            {
                tablesList.Add(
                    new TreeViewJson
                    {
                        text = tables,
                        icon = "fa fa-table fa-fw", 
                        mdaIcon = tables,
                        expand = false,
                        link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Tables/{tables}",
                        selected = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Table,
                      // children = GetTableColumns(tables, astrDatabaseConnection)
                    }
                );
            });
            return tablesList;
        }

        public static IList<TreeViewJson> GetTableColumns(string tables, string astrDatabaseConnection=null)
        {
            var tablesColumns = new List<TreeViewJson>();
            GetTablesColumns(tables.Replace(tables.Substring(0, tables.IndexOf(".")) + ".", ""), IstrDatabaseName, astrDatabaseConnection).
                ForEach(Columns =>
            {
                tablesColumns.Add(
                    new TreeViewJson
                    {
                        text = Columns,
                        icon = "fa fa fa-columns",
                        mdaIcon = Columns,
                        expand = false,
                        link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Tables/{Columns}",
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

        public static TreeViewJson GetViews( string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "Views",
                icon = "fa fa-folder",
                mdaIcon = "Views",
                link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Views",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllViews,
                children = GetViewsChildrens(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetViewsChildrens( string astrDatabaseConnection=null)
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
                        link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Views/{view}",
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

        public static TreeViewJson GetProgrammability(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "Programmability",
                icon = "fa fa-folder",
                mdaIcon = "Programmability",
                link = $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability",
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

        public static TreeViewJson GetStoredProcedures(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "StoredProcedures",
                icon = "fa fa-folder",
                mdaIcon = "StoredProcedures",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/StoredProcedures",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllStoreprocedure,
                children = GetStoredProceduresChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetStoredProceduresChildren(string astrDatabaseConnection =null)
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
                        $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/StoredProcedures/{storeProcedure}",
                    selected = true,
                    badge = 12,
                    SchemaEnums = SchemaEnums.Storeprocedure
                });
            });

            return storeProcedureList;
        }

        public static TreeViewJson GetFunction(string astrDatabaseConnection= null)
        {
            return new TreeViewJson
            {
                text = "Functions",
                icon = "fa fa-folder",
                mdaIcon = "Functions",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions",
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

        public static TreeViewJson GetTableValuedFunctions(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "Table-valued Functions",
                icon = "fa fa-folder",
                mdaIcon = "Table-valued Functions",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/TableValuedFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllTableValueFunction,
                children = GetTableValuedFunctionsChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetTableValuedFunctionsChildren(string astrDatabaseConnection=null)
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
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/TableValuedFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.TableValueFunction
                    }
                );
            });
            return tableValuedFunctions;
        }

        public static TreeViewJson GetScalarValuedFunctions(string astrDatabaseConnection =null)
        {
            return new TreeViewJson
            {
                text = "Scalar-valued Functions",
                icon = "fa fa-folder",
                mdaIcon = "Scalar-valued Functions",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/ScalarValuedFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllScalarValueFunctions,
                children = GetScalarValuedFunctionsChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetScalarValuedFunctionsChildren(string astrDatabaseConnection= null)
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
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/ScalarValuedFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.ScalarValueFunctions
                    }
                );
            });
            return lstScalarFunctions;
        }

        public static TreeViewJson GetAggregateFunctions(string astrDatabaseConnection =null)
        {
            return new TreeViewJson
            {
                text = "Aggregate Functions",
                icon = "fa fa-folder",
                mdaIcon = "Aggregate Functions",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/AggregateFunctions",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllAggregateFunciton,
                children = GetAggregateFunctionsChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetAggregateFunctionsChildren(string astrDatabaseConnection= null)
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
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/AggregateFunctions/{lDatabaseFunctions}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.AggregateFunciton
                    }
                );
            });
            return lstAggregateFunctions;
        }

        public static TreeViewJson GetDatabaseTrigger(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "DatabaseTrigger",
                icon = "fa fa-folder",
                mdaIcon = "DatabaseTrigger",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DatabaseTrigger",
                selected = true,
                expand = true,
                SchemaEnums = SchemaEnums.AllTriggers,
                badge = 12,
                children = GetDatabaseTriggerChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetDatabaseTriggerChildren(string astrDatabaseConnection= null)
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
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DatabaseTrigger/{lDatabaseTrigger}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Triggers
                    }
                );
            });
            return databaseTrigger;
        }

        public static TreeViewJson GetDataBaseDataTypes( string astrDatabaseConnection =null)
        {
            return new TreeViewJson
            {
                text = "Type",
                icon = "fa fa-folder",
                mdaIcon = "Type",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType",
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

        public static TreeViewJson GetUserDefinedDataType( string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "User-Defined Data Types",
                icon = "fa fa-folder",
                mdaIcon = "User Defined Data Types",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/UserDefinedDataType",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllUserDefinedDataType,
                children = GetUserDefinedDataTypeChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetUserDefinedDataTypeChildren( string astrDatabaseConnection=null)
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
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/UserDefinedDataType/{lUserDefinedType}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.UserDefinedDataType
                    }
                );
            });
            return userDefinedType;
        }

        public static TreeViewJson GetXmlSchemas(string astrDatabaseConnection=null)
        {
            return new TreeViewJson
            {
                text = "XML Schema Collections",
                icon = "fa fa-folder",
                mdaIcon = "XML Schema Collections",
                link =
                    $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllXMLSchemaCollection,
                children = GetXmlSchemasChildren(astrDatabaseConnection)
            };
        }

        public static List<TreeViewJson> GetXmlSchemasChildren(string astrDatabaseConnection =null)
        {
            var definedType = new List<TreeViewJson>();
            GetTriggers(IstrDatabaseName, astrDatabaseConnection).ForEach(lXmlSchema =>
            {
                definedType.Add(
                    new TreeViewJson
                    {
                        text = lXmlSchema,
                        //icon = "fa fa-home fa-fw",
                        mdaIcon = lXmlSchema,
                        link =
                            $"/{IstrProjectName}/{astrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection/{lXmlSchema}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.XMLSchemaCollection
                    }
                );
            });
            return definedType;
        }

        #endregion

        #region Storage

        public static TreeViewJson GetStorage()
        {
            return new TreeViewJson();
        }

        public static TreeViewJson GetFullTextCatalogs()
        {
            return new TreeViewJson();
        }

        #endregion

        public static List<string> GetDatabaseName(string astrDatabaseConnection = null)
        {
             
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                 return dbSqldocContext.GetDatabaseNames();
            } 
        }

        public static List<string> GetTables(string astrDatabaseName = null, string astrDatabaseConnection = null)
        {
            return GetTableList(astrDatabaseName, astrDatabaseConnection);
        }

        public static List<string> GetTableList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetTables();
            }
        }

        public static List<string> GetTablesColumns(string astrTableName, string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetTablesColumnsList(astrTableName, astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetTablesColumnsList(string astrTableName, string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetTableColumns(astrTableName);
            }
        }

        //GetTableColumns
        public static List<string> GetTableColumns(string astrColumnName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext())
            {
                return dbSqldocContext.GetTableColumns(astrColumnName);
            }
        }

        public static List<string> GetViews(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetViewsList(astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetViewsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetViewsWithDescription().Select(x=>x.istrName).ToList();
            }
        }

        public static List<string> GetStoreProcedures(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetStoreProceduresList(astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetStoreProceduresList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetStoreProcedures().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetScalarFunctions(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetScalarFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetScalarFunctionsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetScalarFunctions().Where(x => x != null).ToList();
                ;
            }
        }

        public static List<string> GetTableValueFunctions(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetTableValueFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetTableValueFunctionsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetTableValueFunctions().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetAggregateFunctions(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetAggregateFunctionsList(astrDatabaseNames, astrDatabaseConnection);
        }

        public static List<string> GetAggregateFunctionsList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetAggregateFunctions().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetTriggers(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            return GetTriggersList(astrDatabaseNames, astrDatabaseConnection);
        }

        public   static List<string> GetTriggersList(string astrDatabaseNames = null, string astrDatabaseConnection = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetTriggers().Where(x => x.istrName != null).Select(x=>x.istrName).ToList();
            }
        }
        public static List<string> GetUserDefinedTypesList(string astrDatabaseNames = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseNames))
            {
                return dbSqldocContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x=>x.name).ToList();
            }
        }

        public static List<string> GetUserDefinedType(string istrDatabaseName, string astrDatabaseConnection)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x => x.name)
                    .ToList();
            }
        }
    }
     
}