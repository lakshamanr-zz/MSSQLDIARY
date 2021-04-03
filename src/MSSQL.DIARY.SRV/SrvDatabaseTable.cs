using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseTable:SrvMain
    {
        public static NaiveCache<string> CacheThatDependsOn = new NaiveCache<string>();

        public static NaiveCache<List<TablePropertyInfo>> CacheAllTableDetails = new NaiveCache<List<TablePropertyInfo>>();

        public static NaiveCache<List<TableFragmentationDetails>> CacheTableFragmentation = new NaiveCache<List<TableFragmentationDetails>>();
    
        public List<TablePropertyInfo> GetTablesDescription()
        {
            return CacheAllTableDetails.GetOrCreate(istrDatabaseConnection, GetTablesDescriptionFromCache);
        }
         
        /// <summary>
        /// If the table information is not present in cache then create
        /// </summary>
        /// <returns></returns>
        private  List<TablePropertyInfo> GetTablesDescriptionFromCache()
        {
            var lstTableDetails = new List<TablePropertyInfo>(); 
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                dbSqldocContext.GetTablesDescription().GroupBy(x => x.istrName).ToList().ForEach(lstTableProperties =>
                {
                    var lTableProperty = new TablePropertyInfo();
                    foreach (var lTableProperties in lstTableProperties)
                    {
                        lTableProperty.istrFullName = lTableProperties.istrFullName;
                        lTableProperty.istrName = lTableProperties.istrName;
                        lTableProperty.istrSchemaName = lTableProperties.istrSchemaName;
                        lTableProperty.istrNevigation = lTableProperties.istrNevigation;
                        if (lTableProperties.istrValue.Length > 0)
                        {
                            lTableProperty.istrValue += lTableProperties.istrValue;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(lTableProperty.istrValue))
                                lTableProperty.istrValue += ";";
                            else
                                lTableProperty.istrValue += "description of the " + lTableProperties.istrFullName + " is missing.";
                        } 
                        lTableProperty.tableColumns = lTableProperties.tableColumns;
                    }

                    if (!(lTableProperty.istrName.Contains("$") || lTableProperty.istrFullName.Contains("\\") || lTableProperty.istrFullName.Contains("-")))
                                 lstTableDetails.Add(lTableProperty);
                });
            } 

            lstTableDetails.ForEach(tablePropertyInfo =>
            {
                 tablePropertyInfo.tableColumns = GetTableColumns(tablePropertyInfo.istrFullName).DistinctBy(x1 => x1.columnname).ToList(); 
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
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableDescription(astrTableName);
            }
        }

        /// <summary>
        /// Get table Indexes
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableIndexInfo> LoadTableIndexes(string astrTableName )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableIndexes(astrTableName);
            }
        }

        /// <summary>
        /// Get table create script
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public TableCreateScript GetTableCreateScript(string astrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableCreateScript(astrTableName);
            }
        }

        /// <summary>
        /// Get table dependencies
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<Tabledependencies> GetTableDependencies(string astrTableName )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableDependencies(astrTableName);
            }
        }
        /// <summary>
        /// Get column information of the table
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>

        public List<TableColumns> GetTableColumns(string astrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                var lstTableColumns = new List<TableColumns>();
                foreach (var lTableColumnKeyValuePairs in dbSqldocContext.GetTablesColumn(astrTableName).GroupBy(x => x.columnname))
                {
                    var lTableColumn = new TableColumns {columnname = lTableColumnKeyValuePairs.Key}; 
                    foreach (var lTableColumnValue in lTableColumnKeyValuePairs)
                    {
                        lTableColumn.tablename = lTableColumnValue.tablename;
                        lTableColumn.key = lTableColumnValue.key;
                        lTableColumn.identity = lTableColumnValue.identity;
                        lTableColumn.max_length = lTableColumnValue.max_length;
                        lTableColumn.allow_null = lTableColumnValue.allow_null;
                        lTableColumn.defaultValue = lTableColumnValue.defaultValue;
                        lTableColumn.data_type = lTableColumnValue.data_type;
                        lTableColumn.description += lTableColumnValue.description;
                        lTableColumn.HideEdit = false; 
                    }
                    lstTableColumns.Add(lTableColumn);
                }
                int count = 1;
                lstTableColumns.ForEach(tableColumn => 
                {
                    tableColumn.id = count;
                    count++;
                });
                return lstTableColumns;
            }
        }

        /// <summary>
        /// Get Table foreign key details
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableForeignKeys(string astrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableForeignKeys(astrTableName);
            }
        }
        /// <summary>
        /// Get table Key constraints
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTableKeyConstraints(astrTableName);
            }
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
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                dbSqldocContext.CreateOrUpdateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnDescription);
                return true;
            }
        }

        /// <summary>
        /// Create or update the table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="aastrTableName"></param>
        public void CreateOrUpdateTableDescription( string astrDescriptionValue, string astrSchemaName, string aastrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                dbSqldocContext.CreateOrUpdateTableDescription(astrDescriptionValue, astrSchemaName, aastrTableName);
            }
        }

        public string CreatorOrGetDependancyTree(  string astrTableName)
        {
            return CacheThatDependsOn.GetOrCreate
            (
                istrDatabaseConnection + "Table" + astrTableName,
                () =>
                    CreateOrGetCacheTableThatDependenceOn( astrTableName)
            );
        }

        /// <summary>
        /// Cache table related dependency
        /// </summary>
        /// <param name="astrObjectName"></param>
        /// <returns></returns>
        public string CreateOrGetCacheTableThatDependenceOn( string astrObjectName)
        {
            var srvDatabaseObjectDependency = new SrvDatabaseObjectDependency();
            return srvDatabaseObjectDependency.GetJsonResult(srvDatabaseObjectDependency.GetObjectThatDependsOn(istrDatabaseConnection, astrObjectName), srvDatabaseObjectDependency.GetObjectOnWhichDepends(istrDatabaseConnection, astrObjectName), astrObjectName);
        }


        /// <summary>
        /// Cache table related fragmentation details
        /// </summary>
        /// <returns></returns>
        public List<TableFragmentationDetails> CacheTableFragmentationDetails( )
        {
            return CacheTableFragmentation.GetOrCreate(istrDatabaseConnection, GetTablesFragmentation);
        }
        /// <summary>
        /// Get all table fragmentation details
        /// </summary>
        /// <returns></returns>
        private List<TableFragmentationDetails> GetTablesFragmentation( )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetTablesFragmentation();
            }
        }
        /// <summary>
        /// Get Table Fragmentation for the cache
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFragmentationDetails> GetTableFragmentationDetails(  string astrTableName)
        {
            return CacheTableFragmentationDetails().Where(x => x.TableName.Equals(astrTableName)).ToList();
        }
    }
}