using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace SpellWork.Parser
{
    /*public static class Definitions
    {
        public static Definition definiton = new Definition();
        public static bool Load(string path)
        {
            return definiton.LoadDefinition(path);
        }
    }
    public class Definition
    {
        [XmlElement("Table")]
        public HashSet<Table> Tables { get; set; } = new HashSet<Table>();
        [XmlIgnore]
        public int Build { get; set; }
        [XmlIgnore]
        private bool _loading = false;

        public bool LoadDefinition(string path)
        {
            if (_loading) return true;

            try
            {
                XmlSerializer deser = new XmlSerializer(typeof(Definition));
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    Definition def = (Definition)deser.Deserialize(fs);
                    var newtables = def.Tables.Where(x => Tables.Count(y => x.Build == y.Build && x.Name == y.Name) == 0).ToList();
                    newtables.ForEach(x => x.Load());
                    Tables.UnionWith(newtables.Where(x => x.Key != null));
                    return true;
                }
            }
            catch { return false; }
        }
    }*/
}
