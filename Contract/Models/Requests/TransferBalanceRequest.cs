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
    public class TransferBalanceRequest
    {
        public string PersonOCode { get; set; }
        public decimal? Balance { get; set; }

        public bool ResetBalance { get; set; }
    }
}
