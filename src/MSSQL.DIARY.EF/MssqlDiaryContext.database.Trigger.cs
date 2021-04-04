using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MsSqlDiaryContext
    {

        /// <summary>
        /// Get Database Triggers
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetTriggers()
        {
            var propertyInfos = new List<PropertyInfo>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetTriggers;
                    Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                propertyInfos.Add(new PropertyInfo
                                {
                                    istrName = reader.SafeGetString(0),
                                    istrValue = reader.SafeGetString(1)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return propertyInfos;
        }

        /// <summary>
        /// Get Trigger Details by trigger name
        /// </summary>
        /// <param name="astrTriggerName"></param>
        /// <returns></returns>
        public List<TriggerInfo> GetTrigger(string astrTriggerName)
        {
            var triggerInfo = new List<TriggerInfo>();
            try
            {
                using (var lDbConnection = Database.GetDbConnection())
                {
                    var command = lDbConnection.CreateCommand();
                    command.CommandText = SqlQueryConstant.GetTrigger.Replace("@TiggersName", "'" + astrTriggerName + "'");
                    Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            while (reader.Read())
                                triggerInfo.Add(new TriggerInfo
                                {
                                    TiggersName = reader.SafeGetString(0),
                                    TiggersDesc = reader.SafeGetString(1),
                                    TiggersCreateScript = reader.SafeGetString(2),
                                    TiggersCreatedDate = reader.GetDateTime(3).ToString(CultureInfo.InvariantCulture),
                                    TiggersModifyDate = reader.GetDateTime(4).ToString(CultureInfo.InvariantCulture)
                                });
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return triggerInfo;
        }

        /// <summary>
        /// Create or update the trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrTriggerName"></param>
        public void CreateOrUpdateTriggerDescription(string astrDescriptionValue, string astrTriggerName)
        {
            try
            {
                UpdateTriggerDescription(astrDescriptionValue, astrTriggerName);
            }
            catch (Exception)
            {
                CreateTriggerDescription(astrDescriptionValue, astrTriggerName);
            }
        }

        /// <summary>
        /// Update Trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void UpdateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SqlQueryConstant.UpdateTriggerExtendedProperty.Replace("@Trigger_value", "'" + astrDescriptionValue + "'").Replace("@Trigger_Name", "'" + astrSchemaName + "'"); 
                Database.OpenConnection();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create Trigger descriptions
        /// </summary>
        /// <param name="astrDescriptionValue"></param>
        /// <param name="astrSchemaName"></param>
        private void CreateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = SqlQueryConstant.CreateTriggerExtendedProperty.Replace("@Trigger_value", "'" + astrDescriptionValue + "'").Replace("@Trigger_Name", "'" + astrSchemaName + "'");
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
    }
}