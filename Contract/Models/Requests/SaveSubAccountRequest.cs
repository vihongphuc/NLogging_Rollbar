using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SaveSubAccountRequest
    {
        [Required]
        public string Username { get; set; }
        public string Nickname { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required]
        public PersonType Type { get; set; }
        public string Password { get; set; }
        public bool Suspended { get; set; }
        public bool Disabled { get; set; }
                
        public List<KeyValuePair<SubAccountAccessType,bool>> AccessTypes { get; set; }
    }
}
