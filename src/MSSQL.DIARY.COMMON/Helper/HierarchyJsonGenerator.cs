using MSSQL.DIARY.COMN.Models;
using System.Collections.Generic;
using System.Linq;

namespace MSSQL.DIARY.COMN.Helper
{
    public class HierarchyJsonGenerator
    {
        public Node root;

        public HierarchyJsonGenerator(List<string> l, string dependencyName, List<ReferencesModel> referencesModels = null)
        {
            root = new Node(dependencyName) { ReferencesModels = referencesModels };

            foreach (var s in l) AddRow(s);
        }

        public void AddRow(string s)
        {
            var l = s.Split('/').ToList();
            var state = root;
            foreach (var ss in l)
            {
                AddSoon(state, ss);
                state = GetSoon(state, ss);
            }
        }

        private void AddSoon(Node n, string s)
        {
            var f = false;
            foreach (var ns in n.Soon)
                if (ns.name == s)
                    f = !f;
            if (!f) n.Soon.Add(new Node(s));
        }

        private Node GetSoon(Node n, string s)
        {
            foreach (var ns in n.Soon)
                if (ns.name == s)
                    return ns;
            return null;
        }
    }
}