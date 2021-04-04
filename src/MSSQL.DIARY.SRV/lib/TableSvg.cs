using System;
using System.Collections.Generic;
using System.Text;

namespace MSSQL.DIARY.SRV
{
    public class TableSvg
    {
        public TableSvg(string istrTableName, Dictionary<string, string> keyValuePairs)
        {
            TableName = istrTableName;
            ColumnDescription = keyValuePairs;
        }

        private Dictionary<string, string> ColumnDescription { get; }
        public string TableName { get; set; }

        public string IstrTablelLabelStartHtml => "[ shape =none ;label=<<table \tborder=" + "'0'" +
                                                  "\tcellborder=" + "'1'" + "\tcellspacing=" + "'0'" +
                                                  "\tcellpadding=" + "'4'" + ">";

        public string TableHTML => " <tr><td bgcolor=" + "'lightblue'" + ">" + TableName.Split('.')[1] + "</td></tr>";

        public string IstrTablelLabelEndHtml => "</table> >]";

        public string GetTableHtml()
        {
            var tableHtml = "";
            tableHtml += TableName.Split('.')[1];
            tableHtml += IstrTablelLabelStartHtml;
            tableHtml += TableHTML;
            ColumnDescription.ForEach(x =>
            {
                tableHtml += " <tr><td align='left'>" + x.Key + ":" + x.Value + "</td></tr>";
            });
            tableHtml += IstrTablelLabelEndHtml;
            return tableHtml;
        }
    }
}
