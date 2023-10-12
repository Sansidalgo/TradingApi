using System;
using System.Collections.Generic;

namespace SansidAlgo.DataLayer.Models;

public partial class TblTraderDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string PhoneNo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<TblCredential> TblCredentials { get; set; } = new List<TblCredential>();

    public virtual ICollection<TblOrder> TblOrders { get; set; } = new List<TblOrder>();

    public virtual ICollection<TblStrategy> TblStrategies { get; set; } = new List<TblStrategy>();

    public virtual ICollection<TblSubscription> TblSubscriptions { get; set; } = new List<TblSubscription>();

    public virtual ICollection<TblTransactionsHistory> TblTransactionsHistories { get; set; } = new List<TblTransactionsHistory>();
}
