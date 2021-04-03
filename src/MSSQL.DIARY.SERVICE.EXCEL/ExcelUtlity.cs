using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.DIARY.SERVICE.EXCEL
{
    public static class ExcelUtlity
    { /// <summary>
      /// FUNCTION FOR EXPORT TO EXCEL
      /// </summary>
      /// <param name="dataTable"></param>
      /// <param name="worksheetName"></param>
      /// <param name="saveAsLocation"></param>
      /// <returns></returns>
        public static DataTable ReadExcelToDatatble(string astrworksheetName, string astrfilePath, int WorkbookNumber)
        {
            DataTable ldtExcelWorkBook = new DataTable();
            ldtExcelWorkBook.TableName = astrworksheetName;
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(astrfilePath, false))
            {
                //Read the first Sheet from Excel file.
                Sheet sheet =(Sheet) doc.WorkbookPart.Workbook.Sheets.ToList()[WorkbookNumber];

                //Get the Worksheet instance.
                Worksheet worksheet = (doc.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                //Fetch all the rows present in the Worksheet.
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                foreach (Row row in rows)
                {
                    //Use the first row to add columns to DataTable.
                    if (row.RowIndex.Value == 1)
                    {
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            ldtExcelWorkBook.Columns.Add(GetValue(doc, cell));
                        }
                    }
                    else
                    {
                        //Add rows to DataTable.
                        ldtExcelWorkBook.Rows.Add();
                        int i = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            try
                            {
                                ldtExcelWorkBook.Rows[ldtExcelWorkBook.Rows.Count - 1][i] = GetValue(doc, cell);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }

                            i++;
                        }
                    }
                }
                return ldtExcelWorkBook;
            }

        }
        private static string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue?.InnerText??"";
            if (cell!=null && cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

    }

}
