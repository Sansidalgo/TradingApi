using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblRolePlan
{
    public int Id { get; set; }

    public int? PlanId { get; set; }

    public int? RoleId { get; set; }

    public virtual TblPlan? Plan { get; set; }

    public virtual TblRole? Role { get; set; }
}
