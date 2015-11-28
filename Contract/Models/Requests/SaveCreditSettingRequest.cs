using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SaveCreditSettingRequest
    {
        [Required]
        public PersonType Type { get; set; }
        public decimal MaxCredit { get; set; }
    }
}
