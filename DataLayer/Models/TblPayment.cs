using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblPayment
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public int? SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDt { get; set; }

    public int? OfferId { get; set; }

    public bool? Status { get; set; }

    public string? TransactionId { get; set; }

    public virtual TblOffer? Offer { get; set; }

    public virtual TblUserSubscription? Subscription { get; set; }

    public virtual TblTraderDetail? Trader { get; set; }
}
