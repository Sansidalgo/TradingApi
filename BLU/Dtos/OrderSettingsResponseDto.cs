using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class OrderSettingsResponseDto
    {
        public int Id { get; set; }
     
        public string? Broker { get; set; }
        public string? CredentialsName { get; set; }
        public string? OptionsSettingsName { get; set; }
        public TblShoonyaCredential? Credential { get; set; }
        public TblOptionsSetting? OptionsSetting { get; set; }
        public TblBroker? BrokerDetails { get; set; }

    }
}
