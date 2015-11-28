using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class NicknameRequest
    {
        [Required]
        public string Nickname { get; set; }
        [Required]
        public bool IsNickname{ get; set; }
    }
}
