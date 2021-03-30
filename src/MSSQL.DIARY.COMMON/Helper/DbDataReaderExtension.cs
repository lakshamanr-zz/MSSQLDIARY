using System;
using System.Data.Common;

namespace MSSQL.DIARY.COMN.Helper
{
    public static class DbDataReaderExtension
    {
        public static string SafeGetString(this DbDataReader reader, int colIndex)
        {
            if (reader.IsDBNull(colIndex)) return string.Empty;
            try
            {
                return reader.GetString(colIndex);
            }
            catch (Exception)
            {
                try
                {
                    return reader.GetInt32(colIndex).ToString();
                }
                catch (Exception)
                {
                    return reader.GetInt16(colIndex).ToString();
                }
            }
        }
    }
}