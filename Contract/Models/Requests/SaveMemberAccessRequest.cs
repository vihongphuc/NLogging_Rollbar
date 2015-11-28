using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SaveMemberAccessRequest
    {
        public SubAccountAccessType AccessType { get; set; }
        public bool Selected { get; set; }
    }
}
