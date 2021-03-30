using MSSQL.DIARY.COMN.Models;
using System.Collections.Generic;
using System.Linq;

namespace MSSQL.DIARY.COMN.Helper
{
    public class HirechyJsonGenerator
    {
        public Node root;

        public HirechyJsonGenerator(List<string> l, string depancyName, List<ReferencesModel> referencesModels = null)
        {
            root = new Node(depancyName) { ReferencesModels = referencesModels };

            foreach (var s in l) addRow(s);
        }

        public void addRow(string s)
        {
            var l = s.Split('/').ToList();
            var state = root;
            foreach (var ss in l)
            {
                addSoon(state, ss);
                state = getSoon(state, ss);
            }
        }

        private void addSoon(Node n, string s)
        {
            var f = false;
            foreach (var ns in n.Soon)
                if (ns.name == s)
                    f = !f;
            if (!f) n.Soon.Add(new Node(s));
        }

        private Node getSoon(Node n, string s)
        {
            foreach (var ns in n.Soon)
                if (ns.name == s)
                    return ns;
            return null;
        }
    }
}