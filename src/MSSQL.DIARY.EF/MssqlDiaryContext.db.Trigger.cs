using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MSSQL.DIARY.COMN.Constant;
using MSSQL.DIARY.COMN.Helper;
using MSSQL.DIARY.COMN.Models;

namespace MSSQL.DIARY.EF
{
    public partial class MssqlDiaryContext
    {
        public List<PropertyInfo> GetAllDatabaseTrigger()
        {
            var propertyInfos = new List<PropertyInfo>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText = SqlQueryConstant.GetAllDatabaseTrigger;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
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

        public List<TriggerInfo> GetTriggerInfosByName(string istrTriggerName)
        {
            var triggerInfo = new List<TriggerInfo>();
            try
            {
                using (var conn = Database.GetDbConnection())
                {
                    var commad = conn.CreateCommand();
                    commad.CommandText =
                        SqlQueryConstant.GetDatabaseTriggerdtlByName.Replace("@TiggersName",
                            "'" + istrTriggerName + "'");
                    ;
                    Database.OpenConnection();

                    using (var reader = commad.ExecuteReader())
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

        private void UpdateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                commad.CommandText = SqlQueryConstant.UpdateTriggerExtendedProperty
                    .Replace("@Trigger_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Trigger_Name", "'" + astrSchemaName + "'");

                commad.CommandTimeout = 10 * 60;
                Database.OpenConnection();
                commad.ExecuteNonQuery();
            }
        }

        private void CreateTriggerDescription(string astrDescriptionValue, string astrSchemaName)
        {
            using (var commad = Database.GetDbConnection().CreateCommand())
            {
                commad.CommandText = SqlQueryConstant.CreateTriggerExtendedProperty
                    .Replace("@Trigger_value", "'" + astrDescriptionValue + "'")
                    .Replace("@Trigger_Name", "'" + astrSchemaName + "'");

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
    }
}