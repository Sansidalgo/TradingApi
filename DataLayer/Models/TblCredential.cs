using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblCredential
{
    public int Id { get; set; }

    public int BrokerId { get; set; }

    public int BrokerCredentialsId { get; set; }

    public int TraderId { get; set; }

    public virtual TblBroker Broker { get; set; } = null!;

    public virtual ICollection<TblSubscription> TblSubscriptions { get; set; } = new List<TblSubscription>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
