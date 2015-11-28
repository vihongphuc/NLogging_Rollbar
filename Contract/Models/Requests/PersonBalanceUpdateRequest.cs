using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    [JsonObject]
    [Serializable]
    public class PersonBalanceUpdateRequest
    {
        public string PersonOCode { get; set; }

        public decimal? Given { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Available { get; set; }
        public decimal? Outstanding { get; set; }
        public decimal? Transfer { get; set; }
    }
}
