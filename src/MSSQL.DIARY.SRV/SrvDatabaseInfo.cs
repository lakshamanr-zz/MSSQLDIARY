using System.Collections.Generic;
using System.IO;
using System.Linq;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseInfo:SrvMain
    {
           
        /// <summary>
        /// Get set of database object list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get database properties
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseProperties()
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetDatabaseProperties();
            }
        }
         
        /// <summary>
        /// Get database option 
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> GetDatabaseOptions( )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetDatabaseOptions();
            }
        }
        
        /// <summary>
        /// Get database Files
        /// </summary>
        /// <returns></returns>
        public List<FileInfomration> GetDatabaseFiles( )
        {
            using (var dbSqldocContext = new MsSqlDiaryContext(istrDatabaseConnection))
            {
                return dbSqldocContext.GetDatabaseFiles();
            };
        }
         
        public string GetERDiagram(string istrPath, string astrDatabaseConnection, string istrServerName, string istrSchemaName)
        {
             
            var result = File.ReadAllText(GenGraphHtmlString(istrPath+"\\"+ astrDatabaseConnection + ".svg", astrDatabaseConnection, istrSchemaName))
                .Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
            //.Replace("</svg>", "<image xlink:href='https://svgshare.com/i/9Eo.svg' width='1280px' height='560px' ></image></svg>");
            //result = result.Replace("width=", "width=1280px").Replace("height=", " height=600px");
            return result;
        }
        public string GetErDiagram(string astrPath, string astrDatabaseConnection, string istrServerName, string istrSchemaName, List<string> alstSelectedTables)
        {

            var result = File.ReadAllText(GenGraphHtmlString(astrPath + "\\" + astrDatabaseConnection + ".svg", astrDatabaseConnection, istrSchemaName, alstSelectedTables))
                .Replace("<svg", "<svg id='svgDatabaseDiagram' \t");
            return result;
        }
        //
        private string GenGraphHtmlString(string istrPathToStoreSvg, string astrDatabaseConnection, string istrSchemaName)
        {
            try
            {
                astrDatabaseConnection = astrDatabaseConnection.Split('/')[0];
                
            }
            catch (System.Exception)
            {
                // ignored
            }

            File.WriteAllBytes(istrPathToStoreSvg,
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDatabaseConnection, "svg", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".pdf"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDatabaseConnection, "pdf", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".png"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDatabaseConnection, "png", istrSchemaName).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".jpg"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(istrDatabaseConnection, "jpg", istrSchemaName).ToArray());

            return istrPathToStoreSvg;
        }
        private string GenGraphHtmlString(string istrPathToStoreSvg, string astrDatabaseConnection, string istrSchemaName, List<string> alstSelectedTables)
        {
            try
            {
                astrDatabaseConnection = astrDatabaseConnection.Split('/')[0];

            }
            catch (System.Exception)
            {
                // ignored
            }

            File.WriteAllBytes(istrPathToStoreSvg,
                new SrvDatabaseErDiagram().GetGraphHtmlString(astrDatabaseConnection, "svg", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".pdf"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(astrDatabaseConnection, "pdf", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".png"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(astrDatabaseConnection, "png", istrSchemaName, alstSelectedTables).ToArray());
            File.WriteAllBytes(istrPathToStoreSvg.Replace(".svg", ".jpg"),
                new SrvDatabaseErDiagram().GetGraphHtmlString(astrDatabaseConnection, "jpg", istrSchemaName, alstSelectedTables).ToArray());

            return istrPathToStoreSvg;
        }

       
    }
}