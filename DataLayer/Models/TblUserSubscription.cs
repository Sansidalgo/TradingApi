using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblUserSubscription
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public int? PlanId { get; set; }

    public DateOnly StartDt { get; set; }

    public DateOnly? EndDt { get; set; }

    public int SubscriptionStatusId { get; set; }

    public virtual TblPlan? Plan { get; set; }

    public virtual TblSubscriptionStatus SubscriptionStatus { get; set; } = null!;

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();

    public virtual TblTraderDetail? Trader { get; set; }
}
