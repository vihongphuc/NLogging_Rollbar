using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SaveBetStringSettingRequest
    {
        [Required]
        public string BetTypeOCode { get; set; }
        [Required]
        public BetSettingType SettingType { get; set; }
        public string Value { get; set; }
    }
}
