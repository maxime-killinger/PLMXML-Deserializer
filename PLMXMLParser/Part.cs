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
        //Liste des id des sous-références d'instance
        public List<String> ListeIdSousRefs;

        //Liste des sous-références
        public List<Instance> Instances { get; set; }

        //Nom/description
        public string Name { get; set; }

        //Liste des uservalues
        public Dictionary<string, string> UserValues { get; set; }

        //Liste des representations
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
