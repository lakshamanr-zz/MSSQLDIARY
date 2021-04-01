using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;
using System;
using System.Collections.Generic;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {

        /// <summary>
        /// Get list of schemas and there description
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetSchemaWithDescriptions()
        {
            var lstPropInfo = new List<PropertyInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetSchemaWithDescriptions;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstPropInfo.Add(new PropertyInfo
                                    {
                                        istrName = reader.GetString(0),
                                        istrValue = reader.GetString(1)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstPropInfo;
        }

        /// <summary>
        /// Create or update the schema descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        public void CreateOrUpdateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            try
            {
                UpdateSchemaDescription(astrDescriptionValue, astrSchemaName);
            }
            catch (Exception)
            {
                CreateSchemaDescription(astrDescriptionValue, astrSchemaName);
            }
        }

        /// <summary>
        /// Update schema Descriptions 
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void CreateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SqlQueryConstant.CreateSchemaColumnExtendedProperty.Replace("@Schema_info", "'" + astrDescriptionValue + "'").Replace("@SchemaName", "'" + astrSchemaName + "'"); 
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update the schema description
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void UpdateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SqlQueryConstant.UpdateSchemaColumnExtendedProperty.Replace("@Schema_info", "'" + astrDescriptionValue + "'").Replace("@SchemaName", "'" + astrSchemaName + "'"); 
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        } 

        /// <summary>
        /// Get schema references with table / view / store procedures etc. 
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<SchemaReferanceInfo> GetSchemaReferences(string astrSchemaName)
        {
            var lstSchemaReferences = new List<SchemaReferanceInfo>();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetSchemaReferences.Replace("@schema_id", "'" + astrSchemaName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstSchemaReferences.Add(new SchemaReferanceInfo
                                    {
                                        istrName = reader.GetString(0)
                                    }
                                );
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lstSchemaReferences;
        }

        /// <summary>
        /// Get the schema description.
        /// </summary>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public Ms_Description GetSchemaDescription(string astrSchemaName)
        {
            var schemaDescription = new Ms_Description();
            try
            {
                using (var command = Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = SqlQueryConstant.GetSchemaMsDescription.Replace("@schemaName", "'" + astrSchemaName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                schemaDescription.desciption = reader.GetString(0);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return schemaDescription;
        }

        //public SchemaCreateScript GetSchemaCreateSript()
        //{
        //    var sch_cs = new SchemaCreateScript();
        //    try
        //    {
        //        using (var command = Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = SqlQueryConstant.GetServerName;
        //            Database.OpenConnection();
        //            using (var reader = command.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                    while (reader.Read())
        //                    {

        //                        sch_cs.istrCreateScript = reader.GetString(0);
        //                    }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return sch_cs;
        //}
    }
}