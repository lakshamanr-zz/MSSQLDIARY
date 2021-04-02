using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;
using MSSQL.DIARY.ERDIAGRAM;

namespace MSSQL.DIARY.SRV
{
    public class SrvDatabaseErDiagram
    {
        public SrvDatabaseErDiagram()
        {
            IsrvDatabaseTable = new SrvDatabaseTable();
        }

        public SrvDatabaseTable IsrvDatabaseTable { get; set; }


        /// <summary>
        /// Get the  Graph Html string
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrFormatType"></param>
        /// <param name="astrSchemaName"></param>
        /// <returns></returns>
        public byte[] GetGraphHtmlString(string astrDatabaseName, string astrFormatType, string astrSchemaName)
        {
            IsrvDatabaseTable.istrDBConnection = astrDatabaseName;
            // adding sub graph 
            var lGraphSvg = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
              if (astrSchemaName.IsNullOrEmpty())
                IsrvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
                {
                    var columnDictionary = new Dictionary<string, string>();
                    x.tableColumns.ForEach(x2 =>
                    {
                        columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                    });
                    var lTableWithSchema = new TableWithSchema();
                    var ldcTablesAndColumns = new Dictionary<string, Dictionary<string, string>>
                    {
                        {x.istrFullName, columnDictionary}
                    };


                    lTableWithSchema.keyValuePairs = ldcTablesAndColumns;
                    lTableWithSchema.istrSchemaName = x.istrSchemaName;
                    lstTablesAndColumns.Add(lTableWithSchema);
                });
            else
                IsrvDatabaseTable.GetAllDatabaseTablesDescription()
                    .Where(x => x.istrSchemaName == astrSchemaName).ForEach(x =>
                    {
                        var columnDictionary = new Dictionary<string, string>();
                        x.tableColumns.ForEach(x2 =>
                        {
                            columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                        });
                        var lTableWithSchema = new TableWithSchema();
                        var ldcTablesAndColumns = new Dictionary<string, Dictionary<string, string>>
                        {
                            {x.istrFullName, columnDictionary}
                        };


                        lTableWithSchema.keyValuePairs = ldcTablesAndColumns;
                        lTableWithSchema.istrSchemaName = x.istrSchemaName;
                        lstTablesAndColumns.Add(lTableWithSchema);
                    });

            lGraphSvg.SetListOfTables(lstTablesAndColumns, astrSchemaName);

            if (astrFormatType.Equals("pdf"))
                return FileDotEngine.Pdf(lGraphSvg.GraphSVGHTMLString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(lGraphSvg.GraphSVGHTMLString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(lGraphSvg.GraphSVGHTMLString(astrDatabaseName, astrSchemaName));

            return FileDotEngine.Svg(lGraphSvg.GraphSVGHTMLString(astrDatabaseName, astrSchemaName));
        }

        /// <summary>
        /// Get Graph Html string with specific schemaName
        /// </summary>
        /// <param name="astrDatabaseName"></param>
        /// <param name="astrFormatType"></param>
        /// <param name="astrSchemaName"></param>
        /// <param name="alstOfSelectedTables"></param>
        /// <returns></returns>
        public byte[] GetGraphHtmlString(string astrDatabaseName, string astrFormatType, string astrSchemaName,List<string> alstOfSelectedTables)
        {
            var GraphHtml = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
            var lstTablePropertyInfo = new List<TablePropertyInfo>();
            IsrvDatabaseTable.istrDBConnection = astrDatabaseName;
            if (astrSchemaName.IsNullOrEmpty())
            {
                IsrvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
                {
                    if (alstOfSelectedTables.Any(argtbl=>argtbl.Equals(x.istrName)))
                    {
                        lstTablePropertyInfo.Add(x);
                    }
                });
                lstTablePropertyInfo.ForEach(x =>
                   {
                       SelectTableWithOutSchemaNames(x, lstTablesAndColumns);
                   });
            }
            else
            { 
                IsrvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
                {
                    if (alstOfSelectedTables.Any(argtbl => argtbl.Equals(x.istrName)))
                    {
                        lstTablePropertyInfo.Add(x);
                    }
                });

               lstTablePropertyInfo
                    .Where(x => x.istrSchemaName == astrSchemaName)
                    .ForEach(x =>
                    {
                        SelectTableWithSchemaNames(x, lstTablesAndColumns);
                    });
            }

            GraphHtml.SetListOfTables(lstTablesAndColumns, astrSchemaName);

            if (astrFormatType.Equals("pdf"))
                return FileDotEngine.Pdf(GraphHtml.GraphSVGHTMLString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(GraphHtml.GraphSVGHTMLString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(GraphHtml.GraphSVGHTMLString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));

            return FileDotEngine.Svg(GraphHtml.GraphSVGHTMLString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
        }

        /// <summary>
        /// Select Table with sepecified schema
        /// </summary>
        /// <param name="aTablePropertyInfo"></param>
        /// <param name="lstTablesAndColumns"></param>
        private static void SelectTableWithSchemaNames(TablePropertyInfo aTablePropertyInfo, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            aTablePropertyInfo.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var lTableWithSchema = new TableWithSchema();
            var lTablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            lTablesAndColumns.Add(aTablePropertyInfo.istrFullName, columnDictionary);


            lTableWithSchema.keyValuePairs = lTablesAndColumns;
            lTableWithSchema.istrSchemaName = aTablePropertyInfo.istrSchemaName;
            lstTablesAndColumns.Add(lTableWithSchema);
        }

        /// <summary>
        /// Select Table with out schema
        /// </summary>
        /// <param name="aTablePropertyInfo"></param>
        /// <param name="lstTablesAndColumns"></param>
        private static void SelectTableWithOutSchemaNames(TablePropertyInfo aTablePropertyInfo, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            aTablePropertyInfo.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var tableWithSchema = new TableWithSchema();
            var TablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            TablesAndColumns.Add(aTablePropertyInfo.istrFullName, columnDictionary);


            tableWithSchema.keyValuePairs = TablesAndColumns;
            tableWithSchema.istrSchemaName = aTablePropertyInfo.istrSchemaName;
            lstTablesAndColumns.Add(tableWithSchema);
        }

        public class GraphSvg
        {
            private List<TableWithSchema> TablesAndColumns { get; set; }
            public string istrSchemaName { get; set; }

            public string GraphStart => "digraph ERDiagram {  splines=ortho   nodesep=0.8; size=50 ;";

            //  "digraph ERDiagram {  splines=ortho rankdir=LR;  size=50 ";
            public string GraphEnd => "}";
            public GraphNode graphNode { get; set; }
            public GraphEdge graphEdge { get; set; }
            public List<TableSvg> tableSVGs { get; set; }

            public void SetListOfTables(List<TableWithSchema> TablesAndColumn, string istrSchemaName = null)
            {
                TablesAndColumns = TablesAndColumn;
                this.istrSchemaName = istrSchemaName;
            }

            public string GraphSVGHTMLString(string istrdbName, string istrSchemaName)
            {
                string ReturnGraphDot = GenHtmlSting(istrSchemaName);

                ReturnGraphDot += GetAllTableRefernce(istrdbName, istrSchemaName);
                ReturnGraphDot += GraphEnd;

                return ReturnGraphDot;
            }

            public string GraphSVGHTMLString(string istrdbName, string istrSchemaName,List<string> alstOfSelectedTables)
            {
                string ReturnGraphDot = GenHtmlSting(istrSchemaName);

                ReturnGraphDot += GetAllTableRefernce(istrdbName, istrSchemaName, alstOfSelectedTables);
                ReturnGraphDot += GraphEnd;

                return ReturnGraphDot;
            }
            private string GenHtmlSting(string istrSchemaName)
            {
                var node = new GraphNode();
                var edge = new GraphEdge();
                var ReturnGraphDot = "";
                ReturnGraphDot += GraphStart;
                ReturnGraphDot += node.istrGraphNode;
                ReturnGraphDot += edge.istrGraphEdge;
                var clusterIncrement = 0;
                if (istrSchemaName.IsNullOrEmpty())
                    TablesAndColumns.Select(x => x.istrSchemaName).DistinctBy(x => x).ToList().ForEach(x1 =>
                    {
                        TablesAndColumns.Select(x => x.istrSchemaName).DistinctBy(x => x).ToList().ForEach(SchemaName =>
                        {
                            ReturnGraphDot += "subgraph cluster_" + clusterIncrement + " {\t label=" + SchemaName +
                                              ";\t";
                            ReturnGraphDot += "bgcolor=" + "\"" + RandomColor() + "\";";
                            TablesAndColumns.Where(x => x.istrSchemaName.Equals(SchemaName)).ForEach(x =>
                            {
                                x.keyValuePairs.ForEach(x2 =>
                                {
                                    ReturnGraphDot += new TableSvg(x2.Key, x2.Value).GetTableHtml();
                                });
                            });
                            ReturnGraphDot += "\t}\t";
                            clusterIncrement++;
                        });
                    });
                else
                    TablesAndColumns.Select(x => x.istrSchemaName.Equals(istrSchemaName)).DistinctBy(x => x).ToList()
                        .ForEach(x1 =>
                        {
                            TablesAndColumns.Select(x => x.istrSchemaName).DistinctBy(x => x).ToList().ForEach(
                                SchemaName =>
                                {
                                    ReturnGraphDot += "subgraph cluster_" + clusterIncrement + " {\t label=" +
                                                      SchemaName + ";\t";
                                    ReturnGraphDot += "bgcolor=" + "\"" + RandomColor() + "\";";
                                    TablesAndColumns.Where(x => x.istrSchemaName.Equals(SchemaName)).ForEach(x =>
                                    {
                                        x.keyValuePairs.ForEach(x2 =>
                                        {
                                            ReturnGraphDot += new TableSvg(x2.Key, x2.Value).GetTableHtml();
                                        });
                                    });
                                    ReturnGraphDot += "\t}\t";
                                    clusterIncrement++;
                                });
                        });
                return ReturnGraphDot;
            }

            public string RandomColor()
            {
                string[] colorName =
                {
                    "Azure"
                };
                var random = new Random();
                var randomNumber = random.Next(0, colorName.Length - 1);
                return colorName[randomNumber];
            }

            private string GetAllTableRefernce(string istrdbName, string istrSchemaName)
            {
                var RefernceHTML = "";
                var tableReference = new List<TableFKDependency>();
                if (istrSchemaName.IsNullOrEmpty())
                    using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                    {
                        tableReference = dbSqldocContext.GetTableFkReferences();
                    }
                else
                    using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                    {
                        tableReference = dbSqldocContext.GetTableFkReferences(istrSchemaName);
                    }
                
                tableReference.ForEach(x =>
                {
                    RefernceHTML += x.fk_refe_table_name + "\t" + "[fontcolor=block, label=<" + "" +
                                    ">, color =block]";
                });
                return RefernceHTML;
            }
            private string GetAllTableRefernce(string istrdbName, string istrSchemaName, List<string> alstOfSelectedTables)
            {
                var RefernceHTML = "";
                var tableReference = new List<TableFKDependency>();
                if (istrSchemaName.IsNullOrEmpty())
                    using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                    {

                        dbSqldocContext.GetTableFkReferences()
                            .ForEach(x => 
                            {
                                var result = x.fk_refe_table_name.IsNull() ? x.current_table_name : x.fk_refe_table_name;
                                if (alstOfSelectedTables.Any(argtable => result.Contains(argtable)))
                                {
                                    //aTablePropertyInfo.fk_refe_table_name=aTablePropertyInfo.fk_refe_table_name ??"";
                                    //aTablePropertyInfo.fk_refe_table_name = aTablePropertyInfo.fk_refe_table_name.Replace(".", "_");
                                    //aTablePropertyInfo.current_table_name = aTablePropertyInfo.current_table_name ?? "";
                                    //aTablePropertyInfo.current_table_name = aTablePropertyInfo.current_table_name.Replace(".", "_");
                                    tableReference.Add(x);
                                }
                            }
                            );
                    }
                else
                    using (var dbSqldocContext = new MsSqlDiaryContext(istrdbName))
                    { 
                        dbSqldocContext.GetTableFkReferences(istrSchemaName).Where(x=>x.fk_refe_table_name.IsNotNull())
                            .ForEach(x =>
                            {
                                var result = x.fk_refe_table_name.IsNull() ? x.current_table_name : x.fk_refe_table_name;
                                if (alstOfSelectedTables.Any(argtable => result.Contains(argtable)))
                                {
                                    //aTablePropertyInfo.fk_refe_table_name = aTablePropertyInfo.fk_refe_table_name ?? "";
                                    //aTablePropertyInfo.fk_refe_table_name = aTablePropertyInfo.fk_refe_table_name.Replace(".", "_");
                                    //aTablePropertyInfo.current_table_name = aTablePropertyInfo.current_table_name ?? "";
                                    //aTablePropertyInfo.current_table_name = aTablePropertyInfo.current_table_name.Replace(".", "_");
                                    tableReference.Add(x);
                                     
                                }
                            }
                            );
                    }
                tableReference.ForEach(x =>
                {
                    RefernceHTML += x.fk_refe_table_name + "\t" + "[fontcolor=block, label=<" + "" +
                                    ">, color =block]";
                });
                return RefernceHTML;
            }
        }

        public class TableSvg
        {
            public TableSvg(string istrTableName, Dictionary<string, string> keyValuePairs)
            {
                TableName = istrTableName;
                ColumnDescription = keyValuePairs;
            }

            private Dictionary<string, string> ColumnDescription { get; }
            public string TableName { get; set; }

            public string istrTablelLabelStartHtml => "[ shape =none ;label=<<table \tborder=" + "'0'" +
                                                      "\tcellborder=" + "'1'" + "\tcellspacing=" + "'0'" +
                                                      "\tcellpadding=" + "'4'" + ">";

            public string TableHTML => " <tr><td bgcolor=" + "'lightblue'" + ">" + TableName.Split('.')[1] + "</td></tr>";

            public string istrTablelLabelEndHTML => "</table> >]";

            public string GetTableHtml()
            {
                var TableHtml = "";
                TableHtml += TableName.Split('.')[1];
                TableHtml += istrTablelLabelStartHtml;
                TableHtml += TableHTML;
                ColumnDescription.ForEach(x =>
                {
                    TableHtml += " <tr><td align='left'>" + x.Key + ":" + x.Value + "</td></tr>";
                });
                TableHtml += istrTablelLabelEndHTML;
                return TableHtml;
            }
        } 
    }
}