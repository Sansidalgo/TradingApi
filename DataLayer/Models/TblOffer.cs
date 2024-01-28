using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblOffer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();

    public virtual ICollection<TblUserOffer> TblUserOffers { get; set; } = new List<TblUserOffer>();
}
