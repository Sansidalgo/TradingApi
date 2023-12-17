using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblTraderDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual TblRole? Role { get; set; }

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblStrategy> TblStrategies { get; set; } = new List<TblStrategy>();

    public virtual ICollection<TblSubscription> TblSubscriptions { get; set; } = new List<TblSubscription>();

    public virtual ICollection<TblTransactionsHistory> TblTransactionsHistories { get; set; } = new List<TblTransactionsHistory>();

    public virtual ICollection<TblUserPlan> TblUserPlans { get; set; } = new List<TblUserPlan>();
}
