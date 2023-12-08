using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblBroker
{
    public int Id { get; set; }

    public string Broker { get; set; } = null!;

    public virtual ICollection<TblCredential> TblCredentials { get; set; } = new List<TblCredential>();
}
