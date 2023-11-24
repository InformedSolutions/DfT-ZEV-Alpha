#nullable enable
using System;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Zev.Services.ComplianceCalculation.Handler.DTO;

public sealed class RawVehicleDTO
{
    public string Vin { get; set; } = null!;
    public string? Vfn { get; set; }
    public string? Mh { get; set; }
    public string? Man { get; set; }
    public string? MMS { get; set; }
    public string? TAN { get; set; }
    public string? T { get; set; }
    public string? Va { get; set; }
    public string? Ve { get; set; }
    public string? Mk { get; set; }
    public string? Cn { get; set; }
    public string? Ct { get; set; }
    public string? Cr { get; set; }
    public int? M { get; set; }
    public int? MT { get; set; }
    public int? MRVL { get; set; }
    public int? Ewltp { get; set; }
    public int? TPMLM { get; set; }
    public int? W { get; set; }
    public int? At1 { get; set; }
    public int? At2 { get; set; }
    public string? Ft { get; set; }
    public string? Fm { get; set; }
    public int? Ec { get; set; }
    public int? Z { get; set; }
    public string? IT { get; set; }
    public float? Erwltp { get; set; }
    public float? Ber { get; set; }
    public DateOnly DoFr { get; set; }
    public string? SchemeYear { get; set; }
    public string? Postcode { get; set; }
    public string? Spvc { get; set; }
    public bool? Wrm { get; set; }
    public int? Mnp { get; set; }
    public string? Rlce { get; set; }
    public int? Fa { get; set; }
    public string? Trrc { get; set; }
    public string? RegisteredInNation { get; set; }

}