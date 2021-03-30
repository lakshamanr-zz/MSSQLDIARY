namespace MSSQL.DIARY.SERVICE.EXCEL
{
    partial class ExcelService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExcelFileWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelFileWatcher)).BeginInit();
            // 
            // ExcelFileWatcher
            // 
            this.ExcelFileWatcher.EnableRaisingEvents = true;
            // 
            // ExcelService
            // 
            this.ServiceName = "MSSQL.DIARY.SERVICE.EXCEL";
            
            ((System.ComponentModel.ISupportInitialize)(this.ExcelFileWatcher)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher ExcelFileWatcher;
    }
}
