using System.Collections.Generic;

namespace MSSQL.DIARY.COMN.Models
{
    public class ReferencesHierachyModel
    {
        public string label { get; set; }
        public string data { get; set; }
        public string ExpanfObject { get; set; }
        public List<ReferencesHierachyModel> children { get; set; }
    }
}