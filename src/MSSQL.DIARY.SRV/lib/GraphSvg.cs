using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.Extensions;
using MSSQL.DIARY.COMN.Models;
using MSSQL.DIARY.EF;

namespace MSSQL.DIARY.SRV
{
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

        public string GraphSvgHtmlString(string astrdatabaseName, string istrSchemaName, List<string> alstOfSelectedTables)
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
                    dbSqldocContext.GetTableFkReferences(istrSchemaName).Where(x => x.fk_refe_table_name.IsNotNull())
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
}
