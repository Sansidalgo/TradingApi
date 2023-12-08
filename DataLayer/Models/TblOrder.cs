using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOrder
{
    public int Id { get; set; }

    public string Exchange { get; set; } = null!;

    public int TraderId { get; set; }

    public int BrokerId { get; set; }

    public int CredentialId { get; set; }

    public int OrderSideId { get; set; }

    public int SegmentId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public string? ExpiryDay { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public int EnvironmentId { get; set; }

    public int OrderSourceId { get; set; }

    public int StrategyId { get; set; }

    public DateTime CreatedDt { get; set; }

    public int CreatedBy { get; set; }

    public virtual TblEnvironment Environment { get; set; } = null!;

    public virtual TblOrderSide OrderSide { get; set; } = null!;

    public virtual TblOrderSource OrderSource { get; set; } = null!;

    public virtual TblSegment Segment { get; set; } = null!;

    public virtual TblStrategy Strategy { get; set; } = null!;

    public virtual ICollection<TblStatus> TblStatuses { get; set; } = new List<TblStatus>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
