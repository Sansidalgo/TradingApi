using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblStrategy
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TraderId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
