using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblDelegate
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public int? MasterTraderId { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblTraderDetail? CreatedByNavigation { get; set; }

    public virtual TblTraderDetail? UpdatedByNavigation { get; set; }
}
