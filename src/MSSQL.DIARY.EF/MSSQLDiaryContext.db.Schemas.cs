using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public List<PropertyInfo> GetSchemaAndDescriptions()
        {
            var lstPropInfo = new List<PropertyInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText = SqlQueryConstant.GetSchemaAndDescriptions;
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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

        public void CreateOrUpdateSchemaDescription(string astrDescriptionValue, string astrSchemaName)
        {
            try
            {
                UpdateFunctionDescription(astrDescriptionValue, astrSchemaName);
            }
            catch (Exception)
            {
                CreateFunctionDescription(astrDescriptionValue, astrSchemaName);
            }
        }

        private void CreateFunctionDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                commad.CommandText = SqlQueryConstant
                    .CreateSchemaColumnExtendedProperty
                    .Replace("@Schema_info", "'" + astrDescriptionValue + "'")
                    .Replace("@SchemaName", "'" + astrSchemaName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void UpdateFunctionDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                commad.CommandText = SqlQueryConstant
                    .UpdateSchemaColumnExtendedProperty
                    .Replace("@Schema_info", "'" + astrDescriptionValue + "'")
                    .Replace("@SchemaName", "'" + astrSchemaName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        public List<SchemaReferanceInfo> GetSchemaReferences(string astrSchemaName)
        {
            var lstSchemaReferences = new List<SchemaReferanceInfo>();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetSchemaReferences.Replace("@schema_id",
                            "'" + astrSchemaName + "'");
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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

        public Ms_Description GetSchemaDescription(string astrSchemaName)
        {
            var schemaDescription = new Ms_Description();
            try
            {
                using (var commad = Database.GetDbConnection().CreateCommand())
                {
                    commad.CommandText =
                        SqlQueryConstant.GetSchemaMsDescription.Replace("@schemaName", "'" + astrSchemaName + "'");
                    Database.OpenConnection();
                    using (var reader = commad.ExecuteReader())
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
        //        using (var commad = Database.GetDbConnection().CreateCommand())
        //        {
        //            commad.CommandText = SqlQueryConstant.GetServerName;
        //            Database.OpenConnection();
        //            using (var reader = commad.ExecuteReader())
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