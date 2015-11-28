using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class AgTransactionRequest
    {
        public string AccountName { get; set; }
        public bool Ack { get; set; }

        public string AgTransactionOCode { get; set; }

        public string BillNo { get; set; }
        public string DataType { get; set; }
        public string PlatformType { get; set; }
        public string GameType { get; set; }
        public string GameCode { get; set; }

        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }

        public decimal Stake { get; set; }
        public decimal Result { get; set; }
    }
}
