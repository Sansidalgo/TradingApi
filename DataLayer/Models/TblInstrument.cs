using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblInstrument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ExpiryDay { get; set; }

    public int? LotSize { get; set; }

    public virtual ICollection<TblOptionsSetting> TblOptionsSettings { get; set; } = new List<TblOptionsSetting>();
}
