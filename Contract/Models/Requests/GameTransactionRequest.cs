﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class GameTransactionRequest
    {
        public string SessionOCode { get; set; }
        public bool Ack { get; set; }

        public string GameTransactionOCode { get; set; }
        public string GameCode { get; set; }

        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }

        public string GroupOCode { get; set; }

        public decimal StartPointBalance { get; set; }
        public decimal EndPointBalance { get; set; }
        public decimal PointStake { get; set; }
        public decimal PointResult { get; set; }

        public bool NoBroadcast { get; set; }
        public bool NoPoints { get; set; }
        public string SpecialType { get; set; }
    }
}
