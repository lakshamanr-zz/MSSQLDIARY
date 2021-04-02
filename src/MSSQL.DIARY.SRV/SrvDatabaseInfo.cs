using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseInfo
    {
        //private static NaiveCache<List<string>> _avatarCache = new NaiveCache<List<string>>();
        //private static NaiveCache<List<PropertyInfo>> _avatarCache2 = new NaiveCache<List<PropertyInfo>>();
        //private static NaiveCache<List<FileInfomration>> _avatarCache3 = new NaiveCache<List<FileInfomration>>();

        public string istrDBConnection { get; set; }
        public string GetDatabaseUserDefinedText()
        {
            return "";
        }

        public List<string> GetDatabaseObjectTypes()
        {
            return new List<string>
            {
                "Tables",
                "Views",
                "Stored Procedures",
                "Table-valued Functions",
                "Scalar-valued Functions",
                "Database Triggers",
                "User-Defined Data Types",
                "XML Schema Collections",
                "Full Text Catalogs",
                "Users",
                "Database Roles",
                "Schemas" 
            };
        }

        public List<PropertyInfo> GetdbPropertValues()
        {
            return GetdbProperties(istrDBConnection);
        }

        private static List<PropertyInfo> GetdbProperties(string istrdbConn)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetDatabaseProperties();
            }
        }

        public List<PropertyInfo> GetdbOptionValues( )
        {
            return GetdbOptions(istrDBConnection);
        }

        private   List<PropertyInfo> GetdbOptions(string istrdbConn)
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrdbConn))
            {
                return dbSqldocContext.GetDatabaseOptions();
            }
        }

        public List<FileInfomration> GetdbFilesDetails( )
        {
            return GetdbFilesDetail();
        }

        public string GetERDiagram(string istrPath, string istrdbConn, string istrServerName, string istrSchemaName)
        {
             
            var result = File.ReadAllText(GenGraphHtmlString(istrPath+"\\"+ istrdbConn + ".svg", istrdbConn, istrSchemaName))
                .Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
            //.Replace("</svg>", "<image xlink:href='https://svgshare.com/i/9Eo.svg' width='1280px' height='560px' ></image></svg>");
            //result = result.Replace("width=", "width=1280px").Replace("height=", " height=600px");
            return result;
        }
        public string GetERDiagram(string istrPath, string istrdbConn, string istrServerName, string istrSchemaName, List<string> alstOfSelectedTables)
        {

            var result = File.ReadAllText(GenGraphHtmlString(istrPath + "\\" + istrdbConn + ".svg", istrdbConn, istrSchemaName, alstOfSelectedTables))
                .Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
            return result;
        }
        //
        private string GenGraphHtmlString(string istrPathToStoreSVG, string istrdbConn, string istrSchemaName)
        {
            try
            {
                istrdbConn = istrdbConn.Split('/')[0];
                
            }
            catch (System.Exception)
            { 
            }
            File.WriteAllBytes(istrPathToStoreSVG,
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDBConnection, "svg", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".pdf"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDBConnection, "pdf", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".png"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDBConnection, "png", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".jpg"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDBConnection, "jpg", istrSchemaName).ToArray());

            return istrPathToStoreSVG;
        }
        private string GenGraphHtmlString(string istrPathToStoreSVG, string istrdbConn, string istrSchemaName, List<string> alstOfSelectedTables)
        {
            try
            {
                istrdbConn = istrdbConn.Split('/')[0];

            }
            catch (System.Exception)
            {
            }
            File.WriteAllBytes(istrPathToStoreSVG,
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrdbConn, "svg", istrSchemaName, alstOfSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".pdf"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrdbConn, "pdf", istrSchemaName, alstOfSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".png"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrdbConn, "png", istrSchemaName, alstOfSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSVG.Replace(".svg", ".jpg"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrdbConn, "jpg", istrSchemaName, alstOfSelectedTables).ToArray());

            return istrPathToStoreSVG;
        }

        private  List<FileInfomration> GetdbFilesDetail()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDBConnection))
            {
                return dbSqldocContext.GetDatabaseFiles();
            }
        }
    }
}