using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SavePasswordRequest
    {
        [Required]
        public bool CheckOldPassword { get; set; }
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
