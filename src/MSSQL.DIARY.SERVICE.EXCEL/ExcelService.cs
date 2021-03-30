using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MSSQL.DIARY.SERVICE.EXCEL
{
    public partial class ExcelService : ServiceBase
    {
        string WatchPath1 = ConfigurationManager.AppSettings["WatchPath1"];
        System.Timers.Timer timer = new System.Timers.Timer();
        public static bool iblnIsServiceRunning { get; set; }
          public ExcelService()
        {
            InitializeComponent(); 
        } 

        protected override void OnStart(string[] args)
        {
             
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            timer.Interval = 10000; // 10000 ms => 10 second

            timer.Enabled = true;
        } 
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (iblnIsServiceRunning)
            {
                return;
            } 
                string[] allfiles = Directory.GetFiles(WatchPath1, "*.*", SearchOption.AllDirectories);
                foreach (var file in allfiles)
                {
                      iblnIsServiceRunning = true;

                       FileInfo info = new FileInfo(file);
                        try
                        {
                            ExcelFileHandler excelFileHandler = new ExcelFileHandler();
                            excelFileHandler.Start(info.FullName);
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            info.Delete();
                        }
                }
            iblnIsServiceRunning = false;
        } 
    }
}
