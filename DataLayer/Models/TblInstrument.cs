using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblInstrument
{
    public int Id { get; set; }

    public string Instrument { get; set; } = null!;

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();
}
