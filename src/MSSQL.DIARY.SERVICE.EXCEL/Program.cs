using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.DIARY.SERVICE.EXCEL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ExcelFileHandler excelFileHandler = new ExcelFileHandler();
            //excelFileHandler.Start(@"C:\MSSQL\MSSQL.DIARY3.1\src\MSSQL.DIARY.UI.APP\Resources\Excel\SAGITEC-2416\SQLEXPRESS2017\Neospin6.0.2.0\Pilot_1_Financial_Transaction_DB_Design_V2.xlsx");

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ExcelService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
