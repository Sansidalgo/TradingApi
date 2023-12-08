using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblSubscription
{
    public int Id { get; set; }

    public int CredentialsId { get; set; }

    public int TraderId { get; set; }

    public bool Active { get; set; }

    public DateTime StartDt { get; set; }

    public DateTime EndDt { get; set; }

    public virtual TblCredential Credentials { get; set; } = null!;

    public virtual ICollection<TblTransactionsHistory> TblTransactionsHistories { get; set; } = new List<TblTransactionsHistory>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
