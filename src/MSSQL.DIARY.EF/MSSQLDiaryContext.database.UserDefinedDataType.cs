using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {

        /// <summary>
        /// Get list of UserDefined data types
        /// </summary>
        /// <returns></returns>
        public List<UserDefinedDataTypeDetails> GetUserDefinedDataTypes()
        {
            var lstUserDefinedDataTypeDetails = new List<UserDefinedDataTypeDetails>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetUserDefinedDataTypes;
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstUserDefinedDataTypeDetails.Add(new UserDefinedDataTypeDetails
                                {
                                    name = reader.SafeGetString(0),
                                    iblnallownull = reader.GetBoolean(1),
                                    basetypename = reader.SafeGetString(2),
                                    length = reader.GetInt16(3),
                                    createscript = reader.SafeGetString(4)
                                });
                    }
                }
            }
            catch
            {
                // ignored
            }

            return lstUserDefinedDataTypeDetails;
        }

        /// <summary>
        /// Get User defined data type by it names
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public UserDefinedDataTypeDetails GetUserDefinedDataType(string astrTypeName)
        { 
            var lUserDefinedDataTypeDetails = new UserDefinedDataTypeDetails();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetUserDefinedDataTypeDetails.Replace("@TypeName", "'" + astrTypeName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lUserDefinedDataTypeDetails = new UserDefinedDataTypeDetails
                                {
                                    name = reader.SafeGetString(0),
                                    iblnallownull = reader.GetBoolean(1),
                                    basetypename = reader.SafeGetString(2),
                                    length = reader.GetInt16(3),
                                    createscript = reader.SafeGetString(4)
                                };
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lUserDefinedDataTypeDetails;
        }

        /// <summary>
        /// Get user defined data type references.
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public List<UserDefinedDataTypeReferance> GetUsedDefinedDataTypeReference(string astrTypeName)
        {
            var lstUserDefinedDataTypeReference = new List<UserDefinedDataTypeReferance>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetUsedDefinedDataTypeReference.Replace("@TypeName", "'" + astrTypeName + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lstUserDefinedDataTypeReference.Add(new UserDefinedDataTypeReferance
                                {
                                    objectname = reader.SafeGetString(0),
                                    typeofobject = reader.SafeGetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            } 
            return lstUserDefinedDataTypeReference;
        }

        /// <summary>
        ///Get  User defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <returns></returns>
        public Ms_Description GetUsedDefinedDataTypeExtendedProperties(string astrTypeName)
        {
            var lUserDefinedDataTypeDescription = new Ms_Description();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetUsedDefinedDataTypeExtendedProperties.Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                    Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                lUserDefinedDataTypeDescription = new Ms_Description
                                {
                                    desciption = reader.SafeGetString(0)
                                };
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return lUserDefinedDataTypeDescription;
        } 

        /// <summary>
        /// Create User defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescValue"></param>
        private void CreateUsedDefinedDataTypeDescription(string astrTypeName, string astrDescValue)
        {
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.AddUserDefinedDataTypeExtendedProperty.Replace("@desc", "'" + astrDescValue + "'").Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                    Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
        /// <summary>
        /// Update user defined data type description
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescValue"></param>
        private void UpdateUsedDefinedDataTypeDescription(string astrTypeName, string astrDescValue)
        {
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.UpdateUserDefinedDataTypeExtendedProperty.Replace("@desc", "'" + astrDescValue + "'").Replace("@SchemaName", "'" + astrTypeName.Split('.')[0] + "'").Replace("@TypeName", "'" + astrTypeName.Split('.')[1] + "'");
                    Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Create or update the user defined data type extended properties
        /// </summary>
        /// <param name="astrTypeName"></param>
        /// <param name="astrDescriptionValue"></param>
        public void CreateOrUpdateUsedDefinedDataTypeExtendedProperties(string astrTypeName, string astrDescriptionValue)
        {
            try
            {
                  UpdateUsedDefinedDataTypeDescription(astrTypeName, astrDescriptionValue); 
            }
            catch (Exception)
            {   
                CreateUsedDefinedDataTypeDescription(astrTypeName, astrDescriptionValue); 
            }
        }
    }
}