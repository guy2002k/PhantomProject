using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomGui
{
    class JsonRequest 
    {
        public List<string> Ip { get; set; }
        public List<int> Port { get; set; }
        public List<string> Request { get; set; }
    }

    class JsonClientName
    {
        public string Name { get; set; }
    }
}
