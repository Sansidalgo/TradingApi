using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblPlan
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<TblRolePlan> TblRolePlans { get; set; } = new List<TblRolePlan>();

    public virtual ICollection<TblUserPlan> TblUserPlans { get; set; } = new List<TblUserPlan>();

    public virtual ICollection<TblUserSubscription> TblUserSubscriptions { get; set; } = new List<TblUserSubscription>();
}
