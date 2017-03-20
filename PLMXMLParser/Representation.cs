using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLMXMLParser
{
    public class Representation
    {
        public Dictionary<string, string> UserValues { get; set; }
        public string Id { get; set; }
        public string Load { get; set; }

        public Representation()
        {
            UserValues = new Dictionary<string, string>();
        }
    }
}
