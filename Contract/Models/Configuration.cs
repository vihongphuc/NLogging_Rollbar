using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [JsonObject]
    [Serializable]
    public class Configuration
    {
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
