using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOrder
{
    public int Id { get; set; }

    public decimal? IndexPriceAt { get; set; }

    public int? Quantity { get; set; }

    public DateTime CreatedDt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Asset { get; set; }

    public int TraderId { get; set; }

    public int? OrderSideId { get; set; }

    public int? SegmentId { get; set; }

    public int? EnvironmentId { get; set; }

    public int? OrderSourceId { get; set; }

    public int? OrderSettingsId { get; set; }

    public int StatusId { get; set; }

    public decimal? BuyAt { get; set; }

    public decimal? SellAt { get; set; }

    public virtual TblOrderSetting? OrderSettings { get; set; }

    public virtual TblOrderSide? OrderSide { get; set; }

    public virtual TblOrderSource? OrderSource { get; set; }

    public virtual TblSegment? Segment { get; set; }

    public virtual TblStatusType Status { get; set; } = null!;

    public virtual ICollection<TblStatus> TblStatuses { get; set; } = new List<TblStatus>();

    public virtual TblTraderDetail Trader { get; set; } = null!;
}
