﻿using System;
using System.Collections.Generic;

namespace SansidAlgo.DataLayer.Models;

public partial class TblEnvironment
{
    public int Id { get; set; }

    public string Environment { get; set; } = null!;

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();
}