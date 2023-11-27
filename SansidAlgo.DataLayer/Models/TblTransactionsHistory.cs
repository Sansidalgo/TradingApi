using System;
using System.Collections.Generic;

namespace SansidAlgo.DataLayer.Models;

public partial class TblTransactionsHistory
{
    public int Id { get; set; }

    public int SubsriptionId { get; set; }

    public decimal? Amount { get; set; }

    public string? TransactionId { get; set; }

    public string? Source { get; set; }

    public int TraderId { get; set; }

    public DateTime? CreatedDt { get; set; }

    public virtual TblSubscription Subsription { get; set; } = null!;

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
