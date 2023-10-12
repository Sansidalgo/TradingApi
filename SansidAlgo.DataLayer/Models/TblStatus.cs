using System;
using System.Collections.Generic;

namespace SansidAlgo.DataLayer.Models;

public partial class TblStatus
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int StatusTypeId { get; set; }

    public virtual TblOrder Order { get; set; } = null!;

    public virtual TblStatusType StatusType { get; set; } = null!;
}
