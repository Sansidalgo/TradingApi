using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOrderSetting
{
    public int Id { get; set; }

    public int BrokerId { get; set; }

    public int BrokerCredentialsId { get; set; }

    public int TraderId { get; set; }

    public int OptionsSettingsId { get; set; }

    public virtual TblBroker Broker { get; set; } = null!;

    public virtual TblShoonyaCredential BrokerCredentials { get; set; } = null!;

    public virtual TblOptionsSetting OptionsSettings { get; set; } = null!;

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
