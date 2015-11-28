using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class GameTransactionMessageRequest
    {
        public string SessionOCode { get; set; }
        public string GameTransactionOCode { get; set; }
        public string GameCode { get; set; }
        public string GroupOCode { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }

        public decimal PointStake { get; set; }
        public decimal PointResult { get; set; }

        public bool NoPoints { get; set; }
        public string SpecialType { get; set; }
    }
}
