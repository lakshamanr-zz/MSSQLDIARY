using System;
using System.Collections.Generic;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseUserDefinedDataTypes
    {
        /// <summary>
        /// Get details about all used defined data types
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <returns></returns>
        public List<UserDefinedDataTypeDetails> GetUserDefinedDataTypes(string astrDatabaseName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetUserDefinedDataTypes();
            }
        }
        /// <summary>
        /// Get details about specific user defined data type
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="istrTypeName"></param>
        /// <returns></returns>

        public UserDefinedDataTypeDetails GetUserDefinedDataType(string astrDatabaseName, string istrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetUserDefinedDataType(istrTypeName);
            }
        }
        /// <summary>
        /// Get user defined data type references
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>

        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReference(string astrDatabaseName, string astrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetUsedDefinedDataTypeReference(astrTypeName);
            }
        }
        /// <summary>
        /// Get user defined data type extended properties
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="istrTypeName"></param>
        /// <returns></returns>

        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string istrTypeName)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                return dbSqldocContext.GetUsedDefinedDataTypeExtendedProperties(istrTypeName);
            }
        }
        ///
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string astrDatabaseName, string astrTypeName, string astrDescriptionValue)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(astrDatabaseName))
            {
                dbSqldocContext.CreateOrUpdateUsedDefinedDataTypeExtendedProperties(astrTypeName, astrDescriptionValue);
            }
        }
    }
}