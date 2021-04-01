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
    public class srvObjectExplorerDetails
    {
        public static NaiveCache<string> ObjectExplorerDetails = new NaiveCache<string>();
        public SrvDatabaseObjectDependncy srvDatabaseObjectDependncy = new SrvDatabaseObjectDependncy(); 
        public srvObjectExplorerDetails()
        {
            ObjectExplorerDetails = new NaiveCache<string>();
            srvDatabaseObjectDependncy = new SrvDatabaseObjectDependncy();
        }
        public static string IstrProjectName { get; set; }
        public static string IstrServerName { get; set; }
        public static string IstrDatabaseName { get; set; }

        private TreeViewJson SearchInDb()
        {
            return new TreeViewJson
            {
                text = "Search",
                icon = "fa fa-search",
                mdaIcon = "Search",
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables",
                selected = true,
                badge = 12,
                expand = true,
                //SchemaEnums = SchemaEnums.AllTable,
                children = SearchInDbObjects()
            };
        }

        private List<TreeViewJson> SearchInDbObjects()
        {
            var SearchInDbObject = new List<TreeViewJson>
            {
                new TreeViewJson
                {
                    text = "Search In column",
                    icon = "fa fa-search",
                    mdaIcon = "Search In column",
                    expand = false,
                    link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Search/Column",
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
                    link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Search/Column",
                    selected = true,
                    badge = 12
                    //SchemaEnums = SchemaEnums.TableCoumns,
                }
            };
            return SearchInDbObject;
        }

        public static TreeViewJson GetDatabase(string astrdatabaseName = null, string dbConnections = null)
        {
            return new TreeViewJson
            {
                text = astrdatabaseName,
                icon = "fa fa-database fa-fw",
                mdaIcon = astrdatabaseName,
                link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}",
                selected = true,
                badge = 12,
                expand = true,
                SchemaEnums = SchemaEnums.AllDatabase,
                children = new List<TreeViewJson>
                {
                    // SearchInDb(),
                    GetTables(dbConnections),
                    GetViews(dbConnections),
                    GetProgrammability(dbConnections)
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

        public static TreeViewJson GetProjectName(string astrProjectName = null, string astrServerName = null, string astrdatabaseName = null, List<string> astrdbNamelist = null, string dbConnections=null)
        {
            IstrServerName = astrServerName;
            IstrDatabaseName = astrdatabaseName;
            IstrProjectName = astrProjectName;
            return AddDbInformations(astrProjectName, astrServerName, astrdbNamelist, dbConnections);
        }

        public static TreeViewJson AddDbInformations(string astrProjectName, string astrServerName,  List<string> astrdbNamelist = null, string dbConnections=null)
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
                children = new List<TreeViewJson> {GetServerName(astrServerName, astrdbNamelist, dbConnections) }
            }; 
            return leftTreeJoson;
        }

        public static TreeViewJson GetServerName(string istrServerName = null, List<string> astrdbNamelist = null, string dbConnections= null)
        {
            var result = new TreeViewJson
            {
                text = istrServerName,
                icon = "fa  fa-desktop fa-fw",
                mdaIcon = istrServerName,
                link = $"/{IstrProjectName}/{istrServerName}",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.DatabaseServer,
                children = new List<TreeViewJson> {DatabaseInform(astrdbNamelist, dbConnections) }
            };


            return result;
        }

        public static TreeViewJson DatabaseInform(List<string> astrdbNamelist = null, string dbConnections= null)
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
            astrdbNamelist.ForEach(dbInstance =>
            {
                IstrDatabaseName = dbInstance;
                var DatabaseConnection = string.Empty;
                DatabaseConnection += dbConnections?.Split(';')[0] + ";";
                DatabaseConnection += $"Database={IstrDatabaseName};";
                DatabaseConnection += dbConnections?.Split(';')[2] + ";";
                DatabaseConnection += dbConnections?.Split(';')[3] + ";";
                DatabaseConnection += "Trusted_Connection=false;";
                rest.children.Add(GetDatabase(dbInstance, DatabaseConnection));
            });
            return rest;
        } 
        public static List<TreeViewJson> GetObjectExplorer(string dbConnections,string astrDatabaseName=null)
        {
            List<string> lstOfdatabase = new List<string>();
            string lstrDefaultDBName = string.Empty;
            if (astrDatabaseName.IsNotNullOrEmpty())
            {
                lstrDefaultDBName = astrDatabaseName;
                lstOfdatabase.Add(astrDatabaseName);
            }
            else
            {
                lstrDefaultDBName = GetDatabaseName(dbConnections).FirstOrDefault(); 
                 lstOfdatabase = GetDatabaseName(dbConnections);
            }

            var data = new List<TreeViewJson>
            {
                GetProjectName(
                    "Project",
                    dbConnections?.Split(';')[0].Replace("Data Source =", "").Replace("Data Source=", "") ,
                    lstrDefaultDBName,
                    lstOfdatabase,
                    dbConnections
                    )
            };
            return data;
        }
        #region Tables

        public static TreeViewJson GetTables(string dbConnections=null)
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
                children = GetTablesChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetTablesChildren(string dbConnections=null)
        {
            var tablesList = new List<TreeViewJson>();
            GetTables(IstrDatabaseName, dbConnections).ForEach(tables =>
            {
                tablesList.Add(
                    new TreeViewJson
                    {
                        text = tables,
                        icon = "fa fa-table fa-fw", 
                        mdaIcon = tables,
                        expand = false,
                        link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables/{tables}",
                        selected = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Table,
                      // children = GetTableColumns(tables, dbConnections)
                    }
                );
            });
            return tablesList;
        }

        public static IList<TreeViewJson> GetTableColumns(string tables, string dbConnections=null)
        {
            var tablesColumns = new List<TreeViewJson>();
            GetTablesColumns(
                tables.Replace(tables.Substring(0, tables.IndexOf(".")) + ".", ""),
                IstrDatabaseName,
                dbConnections
                ).ForEach(Column =>
            {
                tablesColumns.Add(
                    new TreeViewJson
                    {
                        text = Column,
                        icon = "fa fa fa-columns",
                        mdaIcon = Column,
                        expand = false,
                        link = $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Tables/{Column}",
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

        public static TreeViewJson GetViews( string dbConnections=null)
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
                children = GetViewsChildrens(dbConnections)
            };
        }

        public static List<TreeViewJson> GetViewsChildrens( string dbConnections=null)
        {
            var viewsChildens = new List<TreeViewJson>(); 
            GetViews(IstrDatabaseName, dbConnections).ForEach(view =>
            {
                viewsChildens.Add(
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
            return viewsChildens;
        }

        #endregion

        #region Programmability

        public static TreeViewJson GetProgrammability(string dbConnections=null)
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
                    GetStoredProcedures(dbConnections),
                    GetFunction(dbConnections),
                    GetDatabaseTrigger(dbConnections),
                    GetDataBaseType(dbConnections)
                }
            };
        }

        public static TreeViewJson GetStoredProcedures(string dbConnections=null)
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
                children = GetStoredProceduresChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetStoredProceduresChildren(string dbConnections =null)
        {
            var storeProcedureList = new List<TreeViewJson>();
            GetStoreProcedures(IstrDatabaseName, dbConnections).ForEach(storeProcedure =>
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

        public static TreeViewJson GetFunction(string dbConnections= null)
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
                    GetTableValuedFunctions(dbConnections),
                    GetScalarValuedFunctions(dbConnections),
                    GetAggregateFunctions(dbConnections)
                }
            };
        }

        public static TreeViewJson GetTableValuedFunctions(string dbConnections=null)
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
                children = GetTableValuedFunctionsChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetTableValuedFunctionsChildren(string dbConnections=null)
        {
             var TableValuedFunctions = new List<TreeViewJson>();
                GetTableValueFunctions(IstrDatabaseName, dbConnections).ForEach(Function =>
            {
                TableValuedFunctions.Add(
                    new TreeViewJson
                    {
                        text = Function,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = Function,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/TableValuedFunctions/{Function}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.TableValueFunction
                    }
                );
            });
            return TableValuedFunctions;
        }

        public static TreeViewJson GetScalarValuedFunctions(string dbConnections =null)
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
                children = GetScalarValuedFunctionsChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetScalarValuedFunctionsChildren(string dbConnections= null)
        {
            var ScalarValuedFunctions = new List<TreeViewJson>();

             GetScalarFunctions(IstrDatabaseName, dbConnections).ForEach(Function =>
            {
                ScalarValuedFunctions.Add(
                    new TreeViewJson
                    {
                        text = Function,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = Function,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/ScalarValuedFunctions/{Function}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.ScalarValueFunctions
                    }
                );
            });
            return ScalarValuedFunctions;
        }

        public static TreeViewJson GetAggregateFunctions(string dbConnections =null)
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
                children = GetAggregateFunctionsChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetAggregateFunctionsChildren(string dbConnections= null)
        {
            var AggregateFunctions = new List<TreeViewJson>();
            GetAggregateFunctions(IstrDatabaseName, dbConnections).ForEach(Function =>
            {
                AggregateFunctions.Add(
                    new TreeViewJson
                    {
                        text = Function,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = Function,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/Functions/AggregateFunctions/{Function}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.AggregateFunciton
                    }
                );
            });
            return AggregateFunctions;
        }

        public static TreeViewJson GetDatabaseTrigger(string dbConnections=null)
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
                children = GetDatabaseTriggerChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetDatabaseTriggerChildren(string dbConnections= null)
        {
            var databaseTrigger = new List<TreeViewJson>();
            GetTriggers(IstrDatabaseName, dbConnections).ForEach(trigger =>
            {
                databaseTrigger.Add(
                    new TreeViewJson
                    {
                        text = trigger,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = trigger,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DatabaseTrigger/{trigger}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.Triggers
                    }
                );
            });
            return databaseTrigger;
        }

        public static TreeViewJson GetDataBaseType( string dbConnections =null)
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
                    GetUserDefinedDataType(dbConnections),
                    GetXmlSchemaCollectionTree(dbConnections)
                }
            };
        }

        public static TreeViewJson GetUserDefinedDataType( string dbConnections=null)
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
                children = GetUserDefinedDataTypeChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetUserDefinedDataTypeChildren( string dbConnections=null)
        {
            var userDefinedType = new List<TreeViewJson>();
             GetUserDefinedType(IstrDatabaseName, dbConnections).ForEach(userdefinedfunction =>
            {
                userDefinedType.Add(
                    new TreeViewJson
                    {
                        text = userdefinedfunction,
                        icon = "fa fa-table fa-fw",
                        mdaIcon = userdefinedfunction,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/UserDefinedDataType/{userdefinedfunction}",
                        selected = true,
                        expand = true,
                        badge = 12,
                        SchemaEnums = SchemaEnums.UserDefinedDataType
                    }
                );
            });
            return userDefinedType;
        }

        public static TreeViewJson GetXmlSchemaCollectionTree(string dbConnections=null)
        {
            return new TreeViewJson
            {
                text = "XML Schema Collections",
                icon = "fa fa-folder",
                mdaIcon = "XML Schema Collections",
                link =
                    $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection",
                selected = true,
                expand = true,
                badge = 12,
                SchemaEnums = SchemaEnums.AllXMLSchemaCollection,
                children = GetXmlSchemaCollectionChildren(dbConnections)
            };
        }

        public static List<TreeViewJson> GetXmlSchemaCollectionChildren(string dbConnections =null)
        {
            var definedType = new List<TreeViewJson>();
            GetTriggers(IstrDatabaseName, dbConnections).ForEach(XmlSchema =>
            {
                definedType.Add(
                    new TreeViewJson
                    {
                        text = XmlSchema,
                        //icon = "fa fa-home fa-fw",
                        mdaIcon = XmlSchema,
                        link =
                            $"/{IstrProjectName}/{IstrServerName}/User Database/{IstrDatabaseName}/Programmability/DataBaseType/XmlSchemaCollection/{XmlSchema}",
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

        #region Security

        public static TreeViewJson GetSecurity()
        {
            return new TreeViewJson();
        }

        public static TreeViewJson GetSecurityUsers()
        {
            return new TreeViewJson();
        }

        public static TreeViewJson GetSecurityRoles()
        {
            return new TreeViewJson();
        }

        private TreeViewJson GetSecuritySchema()
        {
            return new TreeViewJson();
        }

        #endregion
      
        public static List<string> GetDatabaseName(string dbConnections = null)
        {
            var lst = new List<string>();
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                lst = dbSqldocContext.GetDatabaseNames().ToList();
            }
            return lst;
        }

        public static List<string> GetTables(string dbInstanceName = null, string dbConnections = null)
        {
            return GetTableList(dbInstanceName, dbConnections);
        }

        public static List<string> GetTableList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetTables().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetTablesColumns(string astrTableName, string dbInstanceName = null, string dbConnections = null)
        {
            return GetTablesColumnsList(astrTableName, dbInstanceName, dbConnections);
        }

        public static List<string> GetTablesColumnsList(string astrTableName, string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
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

        public static List<string> GetViews(string dbInstanceName = null, string dbConnections = null)
        {
            return GetViewsList(dbInstanceName, dbConnections);
        }

        public static List<string> GetViewsList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetViewsWithDescription().Where(x => x.istrName != null).Select(x=>x.istrName).ToList();
            }
        }

        public static List<string> GetStoreProcedures(string dbInstanceName = null, string dbConnections = null)
        {
            return GetStoreProceduresList(dbInstanceName, dbConnections);
        }

        public static List<string> GetStoreProceduresList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetStoreProcedures().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetScalarFunctions(string dbInstanceName = null, string dbConnections = null)
        {
            return GetScalarFunctionsList(dbInstanceName, dbConnections);
        }

        public static List<string> GetScalarFunctionsList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetScalarFunctions().Where(x => x != null).ToList();
                ;
            }
        }

        public static List<string> GetTableValueFunctions(string dbInstanceName = null, string dbConnections = null)
        {
            return GetTableValueFunctionsList(dbInstanceName, dbConnections);
        }

        public static List<string> GetTableValueFunctionsList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetTableValueFunctions().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetAggregateFunctions(string dbInstanceName = null, string dbConnections = null)
        {
            return GetAggregateFunctionsList(dbInstanceName, dbConnections);
        }

        public static List<string> GetAggregateFunctionsList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetAggregateFunctions().Where(x => x != null).ToList();
            }
        }

        public static List<string> GetTriggers(string dbInstanceName = null, string dbConnections = null)
        {
            return GetTriggersList(dbInstanceName, dbConnections);
        }

        public   static List<string> GetTriggersList(string dbInstanceName = null, string dbConnections = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetTriggers().Where(x => x.istrName != null).Select(x=>x.istrName).ToList();
            }
        }
        public static List<string> GetUserDefinedTypesList(string dbInstanceName = null)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbInstanceName))
            {
                return dbSqldocContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x=>x.name).ToList();
            }
        }

        public static List<string> GetUserDefinedType(string istrDatabaseName, string dbConnections)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(dbConnections))
            {
                return dbSqldocContext.GetUserDefinedDataTypes().Where(x => x.name != null).Select(x=>x.name).ToList();
            }
        }

        public List<string> GetFulllTextLogs()
        {
            return new List<string>();
        }

        public List<string> GetDatabaseRoles()
        {
            return new List<string>();
        }

        public List<string> GetSchemas()
        {
            return new List<string>();
        }
    }
     
}