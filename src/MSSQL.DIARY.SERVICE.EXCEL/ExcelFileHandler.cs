using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
namespace MSSQL.DIARY.SERVICE.EXCEL
{
    public class ExcelFileHandler
    {
        string WatchPath1 = ConfigurationManager.AppSettings["WatchPath1"];
        string lstDatabaseConnections = string.Empty;

        public static List<string> lstUpdateCoumnDescitionQuery = new List<string>();
        public static List<string> lstInsertCoumnDescitionQuery = new List<string>();

        public ExcelFileHandler()
        {

        }

        internal void Start()
        {
            
        }
        internal void Start(string astrFileName)
        {
            try
            {
                //specify the file name where its actually exist   
                string filepath = astrFileName;
                DataSet workbookTable = new DataSet();

                LoadDatabaseConnctionString(astrFileName);  

                LoadExcelDataIntoDataTable(filepath, workbookTable);
                CreateUpdateAndInsertQueryForTableAndColumn(workbookTable);

                UpdateColumnDescriptions();
                InsertColumnDescriptions( );

                lstUpdateCoumnDescitionQuery.Clear();
                lstInsertCoumnDescitionQuery.Clear();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void LoadDatabaseConnctionString(string astrFileName)
        {
            var folderInfo = astrFileName.Replace(WatchPath1, "").Split('\\');
            if (folderInfo.Length == 5)
            {
                string lstrServerName = folderInfo[1]+ "\\" +folderInfo[2];
                string lstrDbName = folderInfo[3];
                lstDatabaseConnections = $"Data Source={lstrServerName};User ID=devuser;password=Sagitec11;Initial Catalog={lstrDbName};TimeOut=10;Persist Security Info=True;Asynchronous Processing=True";
            }
            else if (folderInfo.Length==4)
            {
                string lstrServerName = folderInfo[1];
                string lstrDbName = folderInfo[2];
                lstDatabaseConnections = $"Data Source={lstrServerName};User ID=devuser;password=Sagitec11;Initial Catalog={lstrDbName};TimeOut=10;Persist Security Info=True;Asynchronous Processing=True";
            }

        }

        private void InsertColumnDescriptions()
        {
            
            foreach (string query in lstInsertCoumnDescitionQuery)
            {
                SqlConnection sqldbConn = new SqlConnection(lstDatabaseConnections);

                try
                {
                   SqlCommand sqlCommand = new SqlCommand(query, sqldbConn);
                    sqldbConn.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqldbConn.Close();
                   
                }
                catch (Exception)
                {

                }
                finally
                {
                    sqldbConn.Close();
                }
            }

          
        }

        private void UpdateColumnDescriptions()
        {
            foreach (string query in lstUpdateCoumnDescitionQuery)
            {
                SqlConnection sqldbConn = new SqlConnection(lstDatabaseConnections);

                try
                {
                    SqlCommand sqlCommand = new SqlCommand(query, sqldbConn);
                    sqldbConn.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqldbConn.Close(); 
                    lstInsertCoumnDescitionQuery.Remove(query);

                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    sqldbConn.Close();
                }
            }
        }

        private static void CreateUpdateAndInsertQueryForTableAndColumn(DataSet workbookTable)
        {
            foreach (DataTable ldtTableData in workbookTable.Tables)
            {
                if (ldtTableData.TableName.Contains("DS") || ldtTableData.TableName.Contains("FS"))
                {
                    //Check in datatable contains 
                    //Table name / Column name and it description for update.
                    if (ldtTableData.Columns.Contains("TABLE_NAME") &&
                        ldtTableData.Columns.Contains("COLUMN_NAME") &&
                        ldtTableData.Columns.Contains("DESCRIPTION"))
                    {
                        if (ldtTableData.Rows.Count > 0)
                            foreach (DataRow row in ldtTableData.Rows)
                            {
                                string lstrTableName = row["TABLE_NAME"].ToString();
                                string lstrColumnName = row["COLUMN_NAME"].ToString();
                                string lstrDescription = row["DESCRIPTION"].ToString();

                                lstInsertCoumnDescitionQuery.Add(QueryConstants.InsertTableColumnExtendedProperty
                                    .Replace("@Column_value", "'" + lstrDescription + "'")
                                    .Replace("@Schema_Name", "'" + "dbo" + "'")
                                    .Replace("@Table_Name", "'" + lstrTableName + "'")
                                    .Replace("@Column_Name", "'" + lstrColumnName + "'"));

                                lstUpdateCoumnDescitionQuery.Add(QueryConstants.UpdateTableColumnExtendedProperty
                                    .Replace("@Column_value", "'" + lstrDescription + "'")
                                    .Replace("@Schema_Name", "'" + "dbo" + "'")
                                    .Replace("@Table_Name", "'" + lstrTableName + "'")
                                    .Replace("@Column_Name", "'" + lstrColumnName + "'"));
                            }
                    }
                }
            }
        }

        private static void LoadExcelDataIntoDataTable(string filepath, DataSet workbookTable)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filepath, false))
            {

                int sheetNo = 0;
                foreach (var workSheet in doc.WorkbookPart.Workbook.Sheets.ToList())
                {
                    Sheet workbooks = (Sheet)workSheet;
                    workbookTable.Tables.Add(ExcelUtlity.ReadExcelToDatatble(workbooks.Name, filepath, sheetNo));
                    sheetNo++;
                }
            }
        }
    }
}
