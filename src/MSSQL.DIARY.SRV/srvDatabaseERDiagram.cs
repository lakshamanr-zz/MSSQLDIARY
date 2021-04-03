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
    public class SrvDatabaseErDiagram : SrvMain
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
            IsrvDatabaseTable.istrDatabaseConnection = astrDatabaseName;
            // adding sub graph 
            var lGraphSvg = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
              if (astrSchemaName.IsNullOrEmpty())
                IsrvDatabaseTable.GetTablesDescription().ForEach(x =>
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
                IsrvDatabaseTable.GetTablesDescription()
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
                return FileDotEngine.Pdf(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));

            return FileDotEngine.Svg(lGraphSvg.GraphSvgHtmlString(astrDatabaseName, astrSchemaName));
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
            var graphHtml = new GraphSvg();
            var lstTablesAndColumns = new List<TableWithSchema>();
            var lstTablePropertyInfo = new List<TablePropertyInfo>();
            IsrvDatabaseTable.istrDatabaseConnection = astrDatabaseName;
            if (astrSchemaName.IsNullOrEmpty())
            {
                IsrvDatabaseTable.GetTablesDescription().ForEach(x =>
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
                IsrvDatabaseTable.GetTablesDescription().ForEach(x =>
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

            graphHtml.SetListOfTables(lstTablesAndColumns, astrSchemaName);

            if (astrFormatType.Equals("pdf"))
                return FileDotEngine.Pdf(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("png"))
                return FileDotEngine.Png(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
            if (astrFormatType.Equals("jpg"))
                return FileDotEngine.Jpg(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));

            return FileDotEngine.Svg(graphHtml.GraphSvgHtmlString(astrDatabaseName, astrSchemaName, alstOfSelectedTables));
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
            var tablesAndColumns = new Dictionary<string, Dictionary<string, string>>();
            tablesAndColumns.Add(aTablePropertyInfo.istrFullName, columnDictionary);


            tableWithSchema.keyValuePairs = tablesAndColumns;
            tableWithSchema.istrSchemaName = aTablePropertyInfo.istrSchemaName;
            lstTablesAndColumns.Add(tableWithSchema);
        }

        public class GraphSvg
        {
            private List<TableWithSchema> TablesAndColumns { get; set; }
            public string IstrSchemaName { get; set; }

            public string GraphStart => "digraph ERDiagram {  splines=ortho   nodesep=0.8; size=50 ;";

            //  "digraph ERDiagram {  splines=ortho rankdir=LR;  size=50 ";
            public string GraphEnd => "}";

            public void SetListOfTables(List<TableWithSchema> tablesAndColumn, string astrSchemaName = null)
            {
                TablesAndColumns = tablesAndColumn;
                this.IstrSchemaName = astrSchemaName;
            }

            public string GraphSvgHtmlString(string astrdatabaseName, string istrSchemaName)
            {
                string returnGraphDot = GenHtmlString(istrSchemaName);

                returnGraphDot += GetAllTableRefernce(astrdatabaseName, istrSchemaName);
                returnGraphDot += GraphEnd;

                return returnGraphDot;
            }

            public string GraphSvgHtmlString(string astrdatabaseName, string istrSchemaName,List<string> alstOfSelectedTables)
            {
                string returnGraphDot = GenHtmlString(istrSchemaName);

                returnGraphDot += GetAllTableRefernce(astrdatabaseName, istrSchemaName, alstOfSelectedTables);
                returnGraphDot += GraphEnd;

                return returnGraphDot;
            }
            private string GenHtmlString(string istrSchemaName)
            {
                var node = new GraphNode();
                var edge = new GraphEdge();
                var returnGraphDot = "";
                returnGraphDot += GraphStart;
                returnGraphDot += node.istrGraphNode;
                returnGraphDot += edge.istrGraphEdge;
                var clusterIncrement = 0;
                if (istrSchemaName.IsNullOrEmpty())
                    TablesAndColumns.Select(x => x.istrSchemaName).DistinctBy(x => x).ToList().ForEach(x1 =>
                    {
                        TablesAndColumns.Select(x => x.istrSchemaName).DistinctBy(x => x).ToList().ForEach(SchemaName =>
                        {
                            returnGraphDot += "subgraph cluster_" + clusterIncrement + " {\t label=" + SchemaName +
                                              ";\t";
                            returnGraphDot += "bgcolor=" + "\"" + RandomColor() + "\";";
                            TablesAndColumns.Where(x => x.istrSchemaName.Equals(SchemaName)).ForEach(x =>
                            {
                                x.keyValuePairs.ForEach(x2 =>
                                {
                                    returnGraphDot += new TableSvg(x2.Key, x2.Value).GetTableHtml();
                                });
                            });
                            returnGraphDot += "\t}\t";
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
                                    returnGraphDot += "subgraph cluster_" + clusterIncrement + " {\t label=" +
                                                      SchemaName + ";\t";
                                    returnGraphDot += "bgcolor=" + "\"" + RandomColor() + "\";";
                                    TablesAndColumns.Where(x => x.istrSchemaName.Equals(SchemaName)).ForEach(x =>
                                    {
                                        x.keyValuePairs.ForEach(x2 =>
                                        {
                                            returnGraphDot += new TableSvg(x2.Key, x2.Value).GetTableHtml();
                                        });
                                    });
                                    returnGraphDot += "\t}\t";
                                    clusterIncrement++;
                                });
                        });
                return returnGraphDot;
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