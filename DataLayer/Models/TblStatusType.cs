using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblStatusType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblStatus> TblStatuses { get; set; } = new List<TblStatus>();
}
