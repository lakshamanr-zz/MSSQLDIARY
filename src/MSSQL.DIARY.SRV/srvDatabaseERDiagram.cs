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
    public class srvDatabaseERDiagram
    {
        public srvDatabaseERDiagram()
        {
            srvDatabaseTable = new SrvDatabaseTable();
        }

        public SrvDatabaseTable srvDatabaseTable { get; set; }

        public byte[] GetGraphHtmlString(string istrdbName, string FormatType, string istrSchemaName)
        {
            srvDatabaseTable.istrDBConnection = istrdbName;
            // adding sub graph 
            var GraphHtml = new GraphSVG();
            var lstTablesAndColumns = new List<TableWithSchema>();
              if (istrSchemaName.IsNullOrEmpty())
                srvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
                {
                    var columnDictionary = new Dictionary<string, string>();
                    x.tableColumns.ForEach(x2 =>
                    {
                        columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                    });
                    var tableWithSchema = new TableWithSchema();
                    var TablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
                    TablesAndColumns.Add(x.istrFullName, columnDictionary);


                    tableWithSchema.keyValuePairs = TablesAndColumns;
                    tableWithSchema.istrSchemaName = x.istrSchemaName;
                    lstTablesAndColumns.Add(tableWithSchema);
                });
            else
                srvDatabaseTable.GetAllDatabaseTablesDescription()
                    .Where(x => x.istrSchemaName == istrSchemaName).ForEach(x =>
                    {
                        var columnDictionary = new Dictionary<string, string>();
                        x.tableColumns.ForEach(x2 =>
                        {
                            columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
                        });
                        var tableWithSchema = new TableWithSchema();
                        var TablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
                        TablesAndColumns.Add(x.istrFullName, columnDictionary);


                        tableWithSchema.keyValuePairs = TablesAndColumns;
                        tableWithSchema.istrSchemaName = x.istrSchemaName;
                        lstTablesAndColumns.Add(tableWithSchema);
                    });

            GraphHtml.SetListOfTables(lstTablesAndColumns, istrSchemaName);

            if (FormatType.Equals("pdf"))
                return FileDotEngine.PDF(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName));
            if (FormatType.Equals("png"))
                return FileDotEngine.PNG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName));
            if (FormatType.Equals("jpg"))
                return FileDotEngine.JPG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName));

            return FileDotEngine.SVG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName));
        }
        public byte[] GetGraphHtmlString(string istrdbName, string FormatType, string istrSchemaName,List<string> alstOfSelectedTables)
        {
            var GraphHtml = new GraphSVG();
            var lstTablesAndColumns = new List<TableWithSchema>();
            var lstTablePropertyInfo = new List<TablePropertyInfo>();
            srvDatabaseTable.istrDBConnection = istrdbName;
            if (istrSchemaName.IsNullOrEmpty())
            {
                srvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
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
                srvDatabaseTable.GetAllDatabaseTablesDescription().ForEach(x =>
                {
                    if (alstOfSelectedTables.Any(argtbl => argtbl.Equals(x.istrName)))
                    {
                        lstTablePropertyInfo.Add(x);
                    }
                });

               lstTablePropertyInfo
                    .Where(x => x.istrSchemaName == istrSchemaName)
                    .ForEach(x =>
                    {
                        SelecctTableWithSchemaNames(x, lstTablesAndColumns);
                    });
            }

            GraphHtml.SetListOfTables(lstTablesAndColumns, istrSchemaName);

            if (FormatType.Equals("pdf"))
                return FileDotEngine.PDF(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName, alstOfSelectedTables));
            if (FormatType.Equals("png"))
                return FileDotEngine.PNG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName, alstOfSelectedTables));
            if (FormatType.Equals("jpg"))
                return FileDotEngine.JPG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName, alstOfSelectedTables));

            return FileDotEngine.SVG(GraphHtml.GraphSVGHTMLString(istrdbName, istrSchemaName, alstOfSelectedTables));
        }

        private static void SelecctTableWithSchemaNames(TablePropertyInfo x, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            x.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var tableWithSchema = new TableWithSchema();
            var TablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            TablesAndColumns.Add(x.istrFullName, columnDictionary);


            tableWithSchema.keyValuePairs = TablesAndColumns;
            tableWithSchema.istrSchemaName = x.istrSchemaName;
            lstTablesAndColumns.Add(tableWithSchema);
        }

        private static void SelectTableWithOutSchemaNames(TablePropertyInfo x, List<TableWithSchema> lstTablesAndColumns)
        {
            var columnDictionary = new Dictionary<string, string>();
            x.tableColumns.ForEach(x2 =>
            {
                columnDictionary.AddIfNotContainsKey(x2.columnname, x2.data_type);
            });
            var tableWithSchema = new TableWithSchema();
            var TablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            TablesAndColumns.Add(x.istrFullName, columnDictionary);


            tableWithSchema.keyValuePairs = TablesAndColumns;
            tableWithSchema.istrSchemaName = x.istrSchemaName;
            lstTablesAndColumns.Add(tableWithSchema);
        }

        public class GraphSVG
        {
            private List<TableWithSchema> TablesAndColumns { get; set; }
            public string istrSchemaName { get; set; }

            public string GraphStart => "digraph ERDiagram {  splines=ortho   nodesep=0.8; size=50 ;";

            //  "digraph ERDiagram {  splines=ortho rankdir=LR;  size=50 ";
            public string GraphEnd => "}";
            public GraphNode graphNode { get; set; }
            public GraphEdge graphEdge { get; set; }
            public List<TableSVG> tableSVGs { get; set; }

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
                                    ReturnGraphDot += new TableSVG(x2.Key, x2.Value).GetTableHtml();
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
                                            ReturnGraphDot += new TableSVG(x2.Key, x2.Value).GetTableHtml();
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
                    using (var dbSqldocContext = new MssqlDiaryContext(istrdbName))
                    {
                        tableReference = dbSqldocContext.GetTableFkReferences();
                    }
                else
                    using (var dbSqldocContext = new MssqlDiaryContext(istrdbName))
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
                    using (var dbSqldocContext = new MssqlDiaryContext(istrdbName))
                    {

                        dbSqldocContext.GetTableFkReferences()
                            .ForEach(x => 
                            {
                                var result = x.fk_refe_table_name.IsNull() ? x.current_table_name : x.fk_refe_table_name;
                                if (alstOfSelectedTables.Any(argtable => result.Contains(argtable)))
                                {
                                    //x.fk_refe_table_name=x.fk_refe_table_name ??"";
                                    //x.fk_refe_table_name = x.fk_refe_table_name.Replace(".", "_");
                                    //x.current_table_name = x.current_table_name ?? "";
                                    //x.current_table_name = x.current_table_name.Replace(".", "_");
                                    tableReference.Add(x);
                                }
                            }
                            );
                    }
                else
                    using (var dbSqldocContext = new MssqlDiaryContext(istrdbName))
                    { 
                        dbSqldocContext.GetTableFkReferences(istrSchemaName).Where(x=>x.fk_refe_table_name.IsNotNull())
                            .ForEach(x =>
                            {
                                var result = x.fk_refe_table_name.IsNull() ? x.current_table_name : x.fk_refe_table_name;
                                if (alstOfSelectedTables.Any(argtable => result.Contains(argtable)))
                                {
                                    //x.fk_refe_table_name = x.fk_refe_table_name ?? "";
                                    //x.fk_refe_table_name = x.fk_refe_table_name.Replace(".", "_");
                                    //x.current_table_name = x.current_table_name ?? "";
                                    //x.current_table_name = x.current_table_name.Replace(".", "_");
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

        public class GraphNode
        {
            public string istrGraphNode =>
                " node [shape=box, style=filled, color=dodgerblue2, fillcolor=aliceblue];"; //"node [shape=none, margin=0]"; 
        }

        public class GraphEdge
        {
            public string istrGraphEdge => " edge [color=blue4, arrowhead=normal];";
            // " edge [color=blue4, arrowhead=crow];"; 
            //" edge [arrowhead=normal, arrowtail=none, dir=both]"; 
        }

        public class TableSVG
        {
            public TableSVG(string istrTableName, Dictionary<string, string> keyValuePairs)
            {
                TableName = istrTableName;
                ColumnDescription = keyValuePairs;
            }

            private Dictionary<string, string> ColumnDescription { get; }
            public string TableName { get; set; }

            public string istrTablelLabelStartHtml => "[ shape =none ;label=<<table \tborder=" + "'0'" +
                                                      "\tcellborder=" + "'1'" + "\tcellspacing=" + "'0'" +
                                                      "\tcellpadding=" + "'4'" + ">";

            public string TableHTML =>
                " <tr><td bgcolor=" + "'lightblue'" + ">" + TableName.Split('.')[1] + "</td></tr>";

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

        public class TableWithSchema
        {
            public Dictionary<string, Dictionary<string, string>> keyValuePairs { get; set; }
            public string istrSchemaName { get; set; }
        }
    }
}