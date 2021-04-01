using System;
using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseSchema
    {
        public List<PropertyInfo> GetListOfAllSchemaAndMsDescription(string istrdbConn)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetSchemaWithDescriptions();
            }
        }

        public void CreateOrUpdateSchemaMsDescription(string istrdbConn)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
            {
                dbSqldocContext.GetSchemaWithDescriptions();
            }
        }

        public void CreateOrUpdateSchemaMsDescription(string istrdbConn, string astrDescription_Value,
            string astrSchema_Name)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
            {
                dbSqldocContext.CreateOrUpdateSchemaDescription(astrDescription_Value, astrSchema_Name);
            }
        }

        public List<SchemaReferanceInfo> GetSchemaReferanceInfo(string istrdbConn, string astrSchema_Name)
        {
            var result = new List<SchemaReferanceInfo>();
            try
            {
                using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
                {
                    result = dbSqldocContext.GetSchemaReferences(astrSchema_Name);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        public Ms_Description GetSchemaMsDescription(string istrdbConn, string astrSchema_Name)
        {
            var result = new Ms_Description();
            try
            {
                using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
                {
                    result = dbSqldocContext.GetSchemaDescription(astrSchema_Name);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}