using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class OrderSettingsRequestDto
    {
        public int? TraderId { get; set; }
        public int BrokerId { get; set; } 
        public int CredentialsID { get; set; }
        public int OptionsSettingsId { get; set; } 
        public ShoonyaCredentialRequestDto? Credential { get; set; }
        public OptionsSettingRequestDto? OptionsSetting { get; set; }
    }
}
