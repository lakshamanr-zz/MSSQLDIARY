using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Cache;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseTable
    {
        public static NaiveCache<string> CacheThatDependsOn = new NaiveCache<string>();

        public static NaiveCache<List<TablePropertyInfo>> CacheAllTableDetails =
            new NaiveCache<List<TablePropertyInfo>>();

        public static NaiveCache<List<TableFragmentationDetails>> CacheAllTableFragmentationDetails =
            new NaiveCache<List<TableFragmentationDetails>>();
        public string istrDBConnection { get; set; }
        public List<TablePropertyInfo> GetAllDatabaseTablesDescription()
        {
            return CacheAllTableDetails.GetOrCreate(istrDBConnection, () => CreateCacheIfNot());
        }

        private  List<TablePropertyInfo> CreateCacheIfNot()
        {
            var lstTableDetails = new List<TablePropertyInfo>(); 
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                dbSqldocContext.GetTablesDescription().GroupBy(x => x.istrName).ToList().ForEach(Tables =>
                {
                    var tablePropertyInfo = new TablePropertyInfo();
                    foreach (var Table in Tables)
                    {
                        tablePropertyInfo.istrFullName = Table.istrFullName;
                        tablePropertyInfo.istrName = Table.istrName;
                        tablePropertyInfo.istrSchemaName = Table.istrSchemaName;
                        tablePropertyInfo.istrNevigation = Table.istrNevigation;
                        if (Table.istrValue.Length > 0)
                        {
                            tablePropertyInfo.istrValue += Table.istrValue;
                        }
                        else
                        {
                            if (tablePropertyInfo.istrValue != null && tablePropertyInfo.istrValue.Length > 0)
                                tablePropertyInfo.istrValue += ";";
                            else
                                tablePropertyInfo.istrValue +=
                                    "Purpose of the " + Table.istrFullName + " is missing.. ";
                        }

                        tablePropertyInfo.tableColumns = Table.tableColumns;
                    }

                    if (!(tablePropertyInfo.istrName.Contains("$") || tablePropertyInfo.istrFullName.Contains("\\") ||
                          tablePropertyInfo.istrFullName.Contains("-")))
                        
                        lstTableDetails.Add(tablePropertyInfo);
                });
            } 

            lstTableDetails.ForEach(tablePropertyInfo =>
            {
                 tablePropertyInfo.tableColumns = GetAllTablesColumn(tablePropertyInfo.istrFullName).DistinctBy(x1 => x1.columnname).ToList(); 
            });
            return lstTableDetails;
        }

        public Ms_Description GetTableDescription(string istrtableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableDescription(istrtableName);
            }
        }

        public List<TableIndexInfo> LoadTableIndexes(string istrtableName )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableIndexes(istrtableName);
            }
        }

        public TableCreateScript GetTableCreateScript(string istrtableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableCreateScript(istrtableName);
            }
        }

        public List<Tabledependencies> GetAllTabledependencies(string istrtableName )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableDependencies(istrtableName);
            }
        }

        public List<TableColumns> GetAllTablesColumn(string istrtableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                var lsttblcolumn = new List<TableColumns>();
                foreach (var keyValue in dbSqldocContext.GetTablesColumn(istrtableName).GroupBy(x => x.columnname))
                {
                    var tblcolumn = new TableColumns {columnname = keyValue.Key}; 
                    foreach (var values in keyValue)
                    {
                        tblcolumn.tablename = values.tablename;
                        tblcolumn.key = values.key;
                        tblcolumn.identity = values.identity;
                        tblcolumn.max_length = values.max_length;
                        tblcolumn.allow_null = values.allow_null;
                        tblcolumn.defaultValue = values.defaultValue;
                        tblcolumn.data_type = values.data_type;
                        tblcolumn.description += values.description;
                        tblcolumn.HideEdit = false; 
                    }

                    lsttblcolumn.Add(tblcolumn);
                }
                int count = 1;
                lsttblcolumn.ForEach(tblcolumn => 
                {
                    tblcolumn.id = count;
                    count++;
                });
                return lsttblcolumn;
            }
        }

        public List<TableFKDependency> GetAllTableForeignKeys(string istrtableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableForeignKeys(istrtableName);
            }
        }

        public List<TableKeyConstraint> GetTableKeyConstraints(string istrtableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableKeyConstraints(istrtableName);
            }
        }

        public bool CreateOrUpdateColumnDescription(string astrDescription_Value,
            string astrSchema_Name, string astrTableName, string astrColumnValue)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                dbSqldocContext.CreateOrUpdateColumnDescription(astrDescription_Value, astrSchema_Name, astrTableName,
                    astrColumnValue);
                return true;
            }
        }

        public void CreateOrUpdateTableDescription( string astrDescription_Value,
            string astrSchema_Name, string astrTableName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                dbSqldocContext.CreateOrUpdateTableDescription(astrDescription_Value, astrSchema_Name, astrTableName);
            }
        }

        public string CreatorOrGetDependancyTree(  string istrtableName)
        {
            return CacheThatDependsOn.GetOrCreate
            (
                istrDBConnection + "Table" + istrtableName,
                () =>
                    CreateOrGetcacheTableThatDependsOn( istrtableName)
            );
        }

        public string CreateOrGetcacheTableThatDependsOn( string ObjectName)
        {
            var srvDatabaseObjectDependncy = new SrvDatabaseObjectDependncy();
            return srvDatabaseObjectDependncy.JsonResutl(
                srvDatabaseObjectDependncy.GetObjectThatDependsOn(istrDBConnection, ObjectName),
                srvDatabaseObjectDependncy.GetObjectOnWhichDepends(istrDBConnection, ObjectName),
                ObjectName);
        }

        public List<TableFragmentationDetails> CacheTblFramentationDetails( )
        {
            return CacheAllTableFragmentationDetails.GetOrCreate(istrDBConnection,
                () => GetAllTableFragmentationDetails( ));
        }

        private List<TableFragmentationDetails> GetAllTableFragmentationDetails( )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetTableFragmentation();
            }
        }

        public List<TableFragmentationDetails> TableFragmentationDetails(  string istrtableName)
        {
            return CacheTblFramentationDetails().Where(x => x.TableName.Equals(istrtableName)).ToList();
        }
    }
}