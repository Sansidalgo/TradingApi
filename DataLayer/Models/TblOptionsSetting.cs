using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOptionsSetting
{
    public int Id { get; set; }

    public string Instrument { get; set; } = null!;

    public string? ExpiryDay { get; set; }

    public int? LotSize { get; set; }

    public short? CeSideEntryAt { get; set; }

    public short? PeSideEntryAt { get; set; }

    public int? StrategyId { get; set; }

    public virtual TblStrategy? Strategy { get; set; }
}
