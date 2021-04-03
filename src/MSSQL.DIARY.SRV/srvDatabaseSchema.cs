using System;
using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseSchema
    {
        /// <summary>
        /// Get list of schemas and there description
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetSchemaWithDescriptions(string astrDatabaseConnection)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return dbSqldocContext.GetSchemaWithDescriptions();
            }
        }

        /// <summary>
        /// Create or update the schema descriptions
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        public void CreateOrUpdateSchemaMsDescription(string astrDatabaseConnection, string astrDescriptionValue, string astrSchemaName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                dbSqldocContext.CreateOrUpdateSchemaDescription(astrDescriptionValue, astrSchemaName);
            }
        }

        /// <summary>
        /// Get schema references with table / view / store procedures etc. 
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public List<SchemaReferanceInfo> GetSchemaReferences(string astrDatabaseConnection, string astrSchemaName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
                return  dbSqldocContext.GetSchemaReferences(astrSchemaName);
            }
        }

        /// <summary>
        /// Get the schema description.
        /// </summary>
        /// <param name="astrDatabaseConnection"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public Ms_Description GetSchemaDescription(string astrDatabaseConnection, string astrSchemaName)
        { 
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseConnection))
            {
               return dbSqldocContext.GetSchemaDescription(astrSchemaName);
            }  
        }
    }
}