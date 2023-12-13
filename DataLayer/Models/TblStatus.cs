using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblStatus
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int StatusTypeId { get; set; }

    public virtual TblStatusType StatusType { get; set; } = null!;
}
