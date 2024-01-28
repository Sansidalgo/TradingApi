using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
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
     
        public string? Name { get; set; }
        public string? StrategyName { get; set; }
        public int? StrategyId { get; set; }
        public int? InstrumentId { get; set; }
        public int? EnvironmentId { get; set; }
        public string? BrokerName { get; set; }
        public string? OrderSideName { get; set; }
        public string? CredentialsName { get; set; }
        public string? OptionsSettingsName { get; set; }
        public string? InstrumentName { get; set; }
        public string? EnvironmentName { get; set; }
    
        public int TraderId { get; set; }
    
        public int BrokerId { get; set; }
        public int CredentialsID { get; set; }
        public int OptionsSettingsId { get; set; }
        public int OrderSideId { get; set; }
     



        public TblShoonyaCredential? Credential { get; set; }
        public TblOptionsSetting? OptionsSetting { get; set; }
        public TblBroker? Broker { get; set; }
        public TblOrderSide? OrderSide { get; set; }
        public virtual TblTraderDetail Trader { get; set; }
        public virtual TblEnvironment? Environment { get; set; }
        public virtual TblStrategy? Strategy { get; set; }

    }
}
