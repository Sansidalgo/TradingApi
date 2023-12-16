using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblInstrument
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblOptionsSetting> TblOptionsSettings { get; set; } = new List<TblOptionsSetting>();

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();
}
