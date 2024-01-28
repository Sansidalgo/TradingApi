using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblPayment
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public int? SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDt { get; set; }

    public int? OfferId { get; set; }

    public string? TransactionId { get; set; }

    public int? StatusId { get; set; }

    public string? Remarks { get; set; }

    public virtual TblOffer? Offer { get; set; }

    public virtual TblPaymentStatus? Status { get; set; }

    public virtual TblUserSubscription? Subscription { get; set; }

    public virtual TblTraderDetail? Trader { get; set; }
}
