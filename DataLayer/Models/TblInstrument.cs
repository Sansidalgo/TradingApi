using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblInstrument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ExpiryDay { get; set; }

    public int? LotSize { get; set; }

    public string? Exchange { get; set; }

    public virtual ICollection<TblOptionsDatum> TblOptionsData { get; set; } = new List<TblOptionsDatum>();

    public virtual ICollection<TblOptionsSetting> TblOptionsSettings { get; set; } = new List<TblOptionsSetting>();
}
