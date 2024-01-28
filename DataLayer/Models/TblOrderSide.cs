using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOrderSide
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}
