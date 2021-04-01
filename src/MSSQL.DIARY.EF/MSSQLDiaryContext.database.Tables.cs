using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public List<TableIndexInfo> LoadTableIndexes(string istrtableName)
        {
            var tableIndex = new List<TableIndexInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetTableIndex.Replace("@tblName", "'" + istrtableName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableIndex.Add
                                (
                                    new TableIndexInfo
                                    {
                                        index_name = reader.SafeGetString(0),
                                        columns = reader.SafeGetString(1),
                                        index_type = reader.SafeGetString(2),
                                        unique = reader.SafeGetString(3),
                                        tableView = reader.SafeGetString(4),
                                        object_Type = reader.SafeGetString(5)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tableIndex;
        }

        public TableCreateScript GetTableCreateScript(string istrtableName)
        {
            var tableCreateScript = "";
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetTableCreateScript.Replace("@table_name", "'" + istrtableName + "'");
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableCreateScript = reader.GetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new TableCreateScript {createscript = tableCreateScript};
        }

        public List<Tabledependencies> GetAllTableDependencies(string istrtableName)
        {
            var tabledependencies = new List<Tabledependencies>();

            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    try
                    {
                        commad.CommandText =
                            SqlQueryConstant.GetAllTabledependencies.Replace("@tblName", "'" + istrtableName + "'");

                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = commad.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    tabledependencies.Add
                                    (
                                        new Tabledependencies
                                        {
                                            name = reader.SafeGetString(0),
                                            object_type = reader.SafeGetString(1)
                                        }
                                    );
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tabledependencies.DistinctBy(x => x.name).ToList();
        }

        public List<TableColumns> GetAllTablesColumn(string istrtableName)
        {
            var tablecolumns = new List<TableColumns>();

            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetAllTablesColumn.Replace("@tblName", "'" + istrtableName + "'");

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tablecolumns.Add
                                (
                                    new TableColumns
                                    {
                                        tablename = reader.SafeGetString(0),
                                        columnname = reader.SafeGetString(1),
                                        key = reader.SafeGetString(2),
                                        identity = reader.SafeGetString(3),
                                        data_type = reader.SafeGetString(4),
                                        max_length = reader.SafeGetString(5),
                                        allow_null = reader.SafeGetString(6),
                                        defaultValue = reader.SafeGetString(7),
                                        description = reader.SafeGetString(8)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tablecolumns;
        }

        public List<TableFKDependency> GetAllTableForeignKeys(string istrtableName)
        {
            var tableFKcolumns = new List<TableFKDependency>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetAllTableForeignKeys.Replace("@tblName", "'" + istrtableName + "'");

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableFKcolumns.Add
                                (
                                    new TableFKDependency
                                    {
                                        values = reader.SafeGetString(0),
                                        Fk_name = reader.SafeGetString(1),
                                        current_table_name = reader.SafeGetString(3),
                                        current_table_fk_columnName = reader.SafeGetString(4),
                                        fk_refe_table_name = reader.SafeGetString(5),
                                        fk_ref_table_column_name = reader.SafeGetString(6)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tableFKcolumns;
        }

        public List<TableKeyConstraint> GetTableKeyConstraints(string istrtableName)
        {
            var tableKeyConstraints = new List<TableKeyConstraint>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetAllKeyConstraints.Replace("@tblName", "'" + istrtableName + "'");

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableKeyConstraints.Add
                                (
                                    new TableKeyConstraint
                                    {
                                        table_view = reader.SafeGetString(0),
                                        object_type = reader.SafeGetString(1),
                                        Constraint_type = reader.SafeGetString(2),
                                        Constraint_name = reader.SafeGetString(3),
                                        Constraint_details = reader.SafeGetString(4)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tableKeyConstraints;
        }

        public List<TablePropertyInfo> GetAllTableDescription()
        {
            var getAllTbleDesc = new List<TablePropertyInfo>();
            var getlstOfExtendedProps = new List<string>();

            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetListOfExtendedPropList;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getlstOfExtendedProps.Add(reader.SafeGetString(0));
                    }
                }

                getlstOfExtendedProps.ForEach(x =>
                {
                    using (var commad = Database.GetDbConnection().CreateCommand())
                    {
                        commad.CommandText =
                            SqlQueryConstant.GetAllTableDescriptionWithAll.Replace("@ExtendedProp", "'" + x + "'");
                        commad.CommandTimeout = 10 * 60;
                        Database.OpenConnection();
                        using (var reader = commad.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    getAllTbleDesc.Add(new TablePropertyInfo
                                    {
                                        istrName = reader.SafeGetString(0),
                                        istrFullName = reader.SafeGetString(1),
                                        istrValue = reader.SafeGetString(2),
                                        istrSchemaName = reader.SafeGetString(3)
                                        //tableColumns = GetAllTablesColumn(reader.SafeGetString(0))
                                    });
                        }
                    }
                });

                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetAllTableWithOutDesc;
                    commad.CommandTimeout = 10 * 60;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                getAllTbleDesc.Add(new TablePropertyInfo
                                {
                                    istrName = reader.SafeGetString(0),
                                    istrFullName = reader.SafeGetString(1),
                                    istrValue = reader.SafeGetString(2),
                                    istrSchemaName = reader.SafeGetString(3)
                                });
                    }
                } 
                getAllTbleDesc.ForEach(tablePropertyInfo =>
                { 
                    tablePropertyInfo.istrNevigation = GetDatabaseName + "/" + tablePropertyInfo.istrFullName + "/" + GetServerName();
                });
            }
            catch (Exception)
            {
                // ignored
            }


            return getAllTbleDesc;
        }
        public Ms_Description GetTableDescription(string istrtableName)
        {
            var msDesc = "";
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetTableDescription.Replace("@tblName", "'" + istrtableName + "'");

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                msDesc = reader.SafeGetString(1);
                    }
                }

                return new Ms_Description {desciption = msDesc};
            }
            catch (Exception)
            {
                return new Ms_Description {desciption = ""};
            }
        }
        public void CreateOrUpdateTableDescription(string astrDescriptionValue, string astrSchemaName,string astrTableName)
        {
            try
            {
                UpdateTableDescription(astrDescriptionValue, astrSchemaName, astrTableName);
            }
            catch (Exception)
            {
                CreateTableDescription(astrDescriptionValue, astrSchemaName, astrTableName);
            }
        }

        public void CreateOrUpdateColumnDescription(string astrDescriptionValue, string astrSchemaName,string astrTableName, string astrColumnValue)
        {
            try
            {
                UpdateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnValue);
            }
            catch (Exception)
            {
                CreateColumnDescription(astrDescriptionValue, astrSchemaName, astrTableName, astrColumnValue);
            }
        }

        public List<TableFragmentationDetails> GetAllTableFragmentations()
        {
            var tableFrgamentation = new List<TableFragmentationDetails>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.AllTableFragmentation;

                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                tableFrgamentation.Add
                                (
                                    new TableFragmentationDetails
                                    {
                                        TableName = reader.SafeGetString(0),
                                        IndexName = reader.SafeGetString(1),
                                        PercentFragmented = reader.GetInt32(2).ToString()
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return tableFrgamentation.Where(x => Convert.ToInt32(x.PercentFragmented) > 0).ToList();
        }

        private void UpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrTableName.Replace(
                        astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");

                commad.CommandText = SqlQueryConstant
                    .UpdateTableExtendedProperty
                    .Replace("@Table_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void CreateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            var tableName =
                astrTableName.Replace(
                    astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                commad.CommandText = SqlQueryConstant
                    .InsertTableExtendedProperty
                    .Replace("@Table_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'");
                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                try
                {
                    commad.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void UpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrTableName.Replace(
                        astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                commad.CommandText = SqlQueryConstant
                    .UpdateTableColumnExtendedProperty
                    .Replace("@Column_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'")
                    .Replace("@Column_Name", "'" + astrColumnValue + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void CreateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrTableName.Replace(
                        astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                commad.CommandText = SqlQueryConstant
                    .InsertTableColumnExtendedProperty
                    .Replace("@Column_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'")
                    .Replace("@Column_Name", "'" + astrColumnValue + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                try
                {
                    commad.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public List<string> GetTables()
        {
            var lstTables = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTables;
                    command.CommandType = CommandType.StoredProcedure;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                if (!reader.GetString(1).Equals("sys") && reader.GetString(3).Equals("TABLE"))
                                    //lstTables.Add( reader.GetString(2));
                                    lstTables.Add(reader.GetString(1) + "." + reader.GetString(2));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTables;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="istrTableName"></param>
        /// <returns></returns>
        public List<string> GetTableColumns(string istrTableName)
        {
            var lstTableColumns = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableColumns.Replace("@tableName", istrTableName);

                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableColumns.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableColumns;
        }
        /// <summary>
        /// Get table dependencies
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableFkReferences(string astrSchemaName = null)
        {
            var lstTableFkDependencies = new List<TableFKDependency>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    if (astrSchemaName.IsNullOrEmpty())
                    {
                        command.CommandText = SqlQueryConstant.GetTableFkReferences;
                    }
                    else
                    {
                        command.CommandText = SqlQueryConstant.GetTableFkReferencesBySchemaName.Replace("@SchemaName", $"'{astrSchemaName}'");
                        ;
                    }
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableFkDependencies.Add(new TableFKDependency
                                {
                                    Fk_name = reader.GetString(0),
                                    fk_refe_table_name = reader.GetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTableFkDependencies;
        }

    }
}