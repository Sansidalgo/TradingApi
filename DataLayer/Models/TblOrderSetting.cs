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

    public int OrderSideId { get; set; }

    public int? SegmentId { get; set; }

    public int? EnvironmentId { get; set; }

    public int? OrderSourceId { get; set; }

    public string Name { get; set; } = null!;

    public virtual TblBroker Broker { get; set; } = null!;

    public virtual TblShoonyaCredential BrokerCredentials { get; set; } = null!;

    public virtual TblEnvironment? Environment { get; set; }

    public virtual TblOptionsSetting OptionsSettings { get; set; } = null!;

    public virtual TblOrderSide OrderSide { get; set; } = null!;

    public virtual TblOrderSource? OrderSource { get; set; }

    public virtual TblSegment? Segment { get; set; }

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
