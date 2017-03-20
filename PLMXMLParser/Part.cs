using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMXMLParser
{
    public class Part
    {
        public string Id { get; set; }
        public List<String> ListeIdSousRefs;
        public List<Instance> Instances { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> UserValues { get; set; }
        public List<Representation> Representations { get; set; }

        public Part()
        {
            Instances = new List<Instance>();
            ListeIdSousRefs = new List<string>();
            UserValues = new Dictionary<string, string>();
            Representations = new List<Representation>();
        }

    }
}
