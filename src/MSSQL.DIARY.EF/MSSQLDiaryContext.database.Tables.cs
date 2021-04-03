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
    public partial class MsSqlDiaryContext
    {

        /// <summary>
        /// Get list of Index on the table
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableIndexInfo> GetTableIndexes(string astrTableName)
        {
            var lstTableIndexes = new List<TableIndexInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableIndex.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableIndexes.Add
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

            return lstTableIndexes;
        }

        /// <summary>
        /// Get the table create script
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public TableCreateScript GetTableCreateScript(string astrTableName)
        {
            var lstrTableCreateScript = string.Empty;
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableCreateScript.Replace("@table_name", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstrTableCreateScript = reader.GetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new TableCreateScript { createscript = lstrTableCreateScript };
        }

        /// <summary>
        /// Get all the table related dependencies
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>

        public List<Tabledependencies> GetTableDependencies(string astrTableName)
        {
            var lstTableDependencies = new List<Tabledependencies>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableDependencies.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableDependencies.Add
                                (
                                    new Tabledependencies
                                    {
                                        name = reader.SafeGetString(0),
                                        object_type = reader.SafeGetString(1)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return lstTableDependencies.DistinctBy(x => x.name).ToList();
        }

        /// <summary>
        /// Get tables columns details
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableColumns> GetTablesColumn(string astrTableName)
        {
            var lstTablesColumn = new List<TableColumns>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTablesColumn.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTablesColumn.Add
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

            return lstTablesColumn;
        }

        /// <summary>
        /// Get table related foreign keys
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableFKDependency> GetTableForeignKeys(string astrTableName)
        {
            var lstTableFkColumns = new List<TableFKDependency>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableForeignKeys.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableFkColumns.Add
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

            return lstTableFkColumns;
        }


        /// <summary>
        /// Get table Key constraints
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<TableKeyConstraint> GetTableKeyConstraints(string astrTableName)
        {
            var lstTableKeyConstraints = new List<TableKeyConstraint>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetAllKeyConstraints.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableKeyConstraints.Add
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

            return lstTableKeyConstraints;
        }

        /// <summary>
        /// Get tables with descriptions
        /// </summary>
        /// <returns></returns>
        public List<TablePropertyInfo> GetTablesDescription()
        {
            var lstTables = new List<TablePropertyInfo>();
            var lstExtensionProperties = new List<string>();

            try
            {
                try
                {
                    using (var command = Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = SqlQueryConstant.GetListOfExtendedPropertiesList;
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstExtensionProperties.Add(reader.SafeGetString(0));
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                lstExtensionProperties.ForEach(x =>
                {
                    using (var command = Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = SqlQueryConstant.GetTablesWithDescription.Replace("@ExtendedProp", "'" + x + "'");
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstTables.Add(new TablePropertyInfo
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

                try
                {
                    using (var command = Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = SqlQueryConstant.GetTablesWithOutDescription;
                        Database.OpenConnection();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                    lstTables.Add(new TablePropertyInfo
                                    {
                                        istrName = reader.SafeGetString(0),
                                        istrFullName = reader.SafeGetString(1),
                                        istrValue = reader.SafeGetString(2),
                                        istrSchemaName = reader.SafeGetString(3)
                                    });
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                lstTables.ForEach(tablePropertyInfo =>
                {
                    tablePropertyInfo.istrNevigation = GetDatabaseName + "/" + tablePropertyInfo.istrFullName + "/" + GetServerName();
                });
            }
            catch (Exception)
            {
                // ignored
            }

            return lstTables;
        }

        //Get table descriptions
        public Ms_Description GetTableDescription(string astrTableName)
        {
            var lstrTableDescription = string.Empty;
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableDescription.Replace("@tblName", "'" + astrTableName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new Ms_Description { desciption = lstrTableDescription };
                        while (reader.Read())
                            lstrTableDescription = reader.SafeGetString(1);
                    }
                }

                return new Ms_Description { desciption = lstrTableDescription };
            }
            catch (Exception)
            {
                return new Ms_Description { desciption = "" };
            }
        }

        /// <summary>
        /// Create or Update Table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        public void CreateOrUpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
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

        /// <summary>
        /// Update the table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        private void UpdateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var tableName =
                    astrTableName.Replace(
                        astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");

                command.CommandText = SqlQueryConstant
                    .UpdateTableExtendedProperty
                    .Replace("@Table_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'");

                command.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create table descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        private void CreateTableDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName)
        {
            var tableName =
                astrTableName.Replace(
                    astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SqlQueryConstant
                    .InsertTableExtendedProperty
                    .Replace("@Table_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Schema_Name", "'" + astrSchemaName + "'")
                    .Replace("@Table_Name", "'" + tableName + "'");
                command.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }


        /// <summary>
        /// Create or update table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        public void CreateOrUpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
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

        /// <summary>
        /// Update table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        private void UpdateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var lstrTableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.UpdateTableColumnExtendedProperty.Replace("@Column_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + lstrTableName + "'").Replace("@Column_Name", "'" + astrColumnValue + "'");
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create table column descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="astrTableName"></param>
        /// <param name="astrColumnValue"></param>
        private void CreateColumnDescription(string astrDescriptionValue, string astrSchemaName, string astrTableName, string astrColumnValue)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                var lstrTableName = astrTableName.Replace(astrTableName.Substring(0, astrTableName.IndexOf(".", StringComparison.Ordinal)) + ".", "");
                command.CommandText = SqlQueryConstant.InsertTableColumnExtendedProperty.Replace("@Column_value", "'" + astrDescriptionValue + "'").Replace("@Schema_Name", "'" + astrSchemaName + "'").Replace("@Table_Name", "'" + lstrTableName + "'").Replace("@Column_Name", "'" + astrColumnValue + "'");
                Database.OpenConnection();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Get table Fragmentation details
        /// </summary>
        /// <returns></returns>
        public List<TableFragmentationDetails> GetTablesFragmentation()
        {
            var lstTableFragmentation = new List<TableFragmentationDetails>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.TableFragmentation;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstTableFragmentation.Add
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

            return lstTableFragmentation.Where(x => Convert.ToInt32(x.PercentFragmented) > 0).ToList();
        }

        /// <summary>
        /// Get table details
        /// </summary>
        /// <returns></returns>
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
        /// Get table columns
        /// </summary>
        /// <param name="astrTableName"></param>
        /// <returns></returns>
        public List<string> GetTableColumns(string astrTableName)
        {
            var lstTableColumns = new List<string>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetTableColumns.Replace("@tableName", astrTableName);
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