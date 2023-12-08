using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOrderSide
{
    public int Id { get; set; }

    public string? Side { get; set; }

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}
