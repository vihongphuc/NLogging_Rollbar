using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sbs.Models
{
    public class SavePersonRequest
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
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Suspended { get; set; }
        public bool Disabled { get; set; }        
        public bool AutoTransfer { get; set; }
        public bool External { get; set; }

        [Display(Name = "Given Credit")]
        [Required]
        public decimal GivenCredit { get; set; }

        public IList<SaveBetNumberSettingRequest> BetNumberSettings { get; set; }
        public IList<SaveBetStringSettingRequest> BetStringSettings { get; set; }
        public IList<SaveCreditSettingRequest> CreditSettings { get; set; }
        public IList<PositionTaking> PositionTakings { get; set; }
        public SaveTransferSettingRequest TransferSetting { get; set; }
    }
}
