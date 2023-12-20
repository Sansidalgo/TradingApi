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
        public bool IsEditing { get; set; }
        public string? Name {  get; set; }
        public int? Id { get; set; }
        public int TraderId { get; set; }
        public int StrategyId { get; set; }
        public string StrategyName { get; set; }
        public int BrokerId { get; set; } 
        public int CredentialsID { get; set; }
        public int OptionsSettingsId { get; set; }
        public int OrderSideId { get; set; }
        public int EnvironmentId { get; set; }
        public ShoonyaCredentialRequestDto? Credential { get; set; }
        public OptionsSettingRequestDto? OptionsSetting { get; set; }

        public virtual TblStrategy? Strategy { get; set; }
    }
}
