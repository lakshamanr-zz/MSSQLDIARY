using System;
using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class srvDatabaseUserDefinedDataTypes
    {
        public List<UserDefinedDataTypeDetails> GetAllUserDefinedDataTypes(string istrdbName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetUserDefinedDataTypes();
            }
        }

        public UserDefinedDataTypeDetails GetUserDefinedDataTypeDetails(string istrdbName, string istrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetUserDefinedDataType(istrTypeName);
            }
        }

        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReferance(string istrdbName,
            string istrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetUsedDefinedDataTypeReference(istrTypeName);
            }
        }

        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string istrdbName, string istrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
            {
                return dbSqldocContext.GetUsedDefinedDataTypeExtendedProperties(istrTypeName);
            }
        }

        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string istrdbName, string istrTypeName,
            string istrdescValue)
        {
            try
            {
                using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                {
                    dbSqldocContext.UpdateUsedDefinedDataTypeDescription(istrTypeName, istrdescValue);
                }
            }
            catch (Exception ex)
            {
                using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                {
                    dbSqldocContext.CreateUsedDefinedDataTypeDescription(istrTypeName, istrdescValue);
                }
            }

            //
        }
    }
}