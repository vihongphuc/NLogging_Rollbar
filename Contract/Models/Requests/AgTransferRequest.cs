using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class AgTransferRequest
    {
        public string GameSessionOCode { get; set; }
        public decimal Amount { get; set; }
    }
}
