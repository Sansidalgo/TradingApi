using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblStrategy
{
    public int Id { get; set; }

    public string Strategy { get; set; } = null!;

    public int TraderId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<TblOptionsSetting> TblOptionsSettings { get; set; } = new List<TblOptionsSetting>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
