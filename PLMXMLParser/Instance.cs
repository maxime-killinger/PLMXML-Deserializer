using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMXMLParser
{
    public class Instance
    {
        public string Id { get; set; }
        public string Partref { get; set; }
        //Pièce de l'instance
        public Part Piece { get; set; }
        public Dictionary<string, string> UserValues { get; set; }

        public Instance()
        {
            UserValues = new Dictionary<string, string>();
        }
    }
}
