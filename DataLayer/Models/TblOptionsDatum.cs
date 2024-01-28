using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOptionsDatum
{
    public int Id { get; set; }

    public DateTime? EntryDateTime { get; set; }

    public double? PcrOi { get; set; }

    public double? PcrOichange { get; set; }

    public int? PutOi { get; set; }

    public int? CallOi { get; set; }

    public int? PutOichange { get; set; }

    public int? CallOichange { get; set; }

    public double? Pevwap { get; set; }

    public double? Cevwap { get; set; }
}
