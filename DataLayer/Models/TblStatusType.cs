using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblStatusType
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TblStatus> TblStatuses { get; set; } = new List<TblStatus>();
}
