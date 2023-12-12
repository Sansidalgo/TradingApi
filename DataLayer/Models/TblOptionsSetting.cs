using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOptionsSetting
{
    public int Id { get; set; }

    public string Instrument { get; set; } = null!;

    public string? ExpiryDay { get; set; }

    public short? CeSideEntryAt { get; set; }

    public short? PeSideEntryAt { get; set; }

    public int? StrategyId { get; set; }

    public int TraderId { get; set; }

    public DateTime? CreatedDt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Exchange { get; set; }

    public int? LotSize { get; set; }

    public virtual TblStrategy? Strategy { get; set; }

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
