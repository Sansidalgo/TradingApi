using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblPaymentStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();
}
