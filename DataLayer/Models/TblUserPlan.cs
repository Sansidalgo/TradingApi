using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblUserPlan
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public virtual TblPlan Plan { get; set; } = null!;

    public virtual TblTraderDetail User { get; set; } = null!;
}
