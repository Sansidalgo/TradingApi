using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblRolePlan> TblRolePlans { get; set; } = new List<TblRolePlan>();

    public virtual ICollection<TblTraderDetail> TblTraderDetails { get; set; } = new List<TblTraderDetail>();
}
