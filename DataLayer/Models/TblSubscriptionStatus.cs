using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblSubscriptionStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblUserSubscription> TblUserSubscriptions { get; set; } = new List<TblUserSubscription>();
}
