using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblBroker
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblOrderSetting> TblOrderSettings { get; set; } = new List<TblOrderSetting>();
}
