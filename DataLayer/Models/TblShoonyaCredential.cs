using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class TblShoonyaCredential
{
    public int Id { get; set; }

    public int? TraderId { get; set; }

    public string? Uid { get; set; }

    public string? Password { get; set; }

    public string? AuthSecreteKey { get; set; }

    public string? Imei { get; set; }

    public string? Vc { get; set; }

    public string? ApiKey { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDt { get; set; }

    public DateTime? UpdatedDt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }
}
