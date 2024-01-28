using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblUserOffer
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public int? OfferId { get; set; }

    public DateTime? AppliedDate { get; set; }

    public virtual TblOffer? Offer { get; set; }

    public virtual TblTraderDetail? Trader { get; set; }
}
