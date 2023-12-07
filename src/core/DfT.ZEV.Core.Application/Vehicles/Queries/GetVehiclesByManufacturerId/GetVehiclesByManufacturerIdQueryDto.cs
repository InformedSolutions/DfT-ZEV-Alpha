namespace DfT.ZEV.Core.Application.Vehicles.Queries.GetVehiclesByManufacturerIdQuery;

public class VehicleListElementDto
{
  public string Vin { get; set; } = null!;
  public string Vfn { get; set; } = null!;
  public string Mh { get; set; } = null!;
  public string Man { get; set; } = null!;
  public string MMS { get; set; } = null!;
  public string TAN { get; set; } = null!;
  public string T { get; set; } = null!;
  public string Va { get; set; } = null!;
  public string Ve { get; set; } = null!;
  public string Mk { get; set; } = null!;
  public string Cn { get; set; } = null!;
  public string Ct { get; set; } = null!;
  public string Cr { get; set; } = null!;
  public int M { get; set; }
  public int MT { get; set; }
  public int? MRVL { get; set; }
  public int Ewltp { get; set; }
  public int TPMLM { get; set; }
  public int W { get; set; }
  public int At1 { get; set; }
  public int At2 { get; set; }
  public string Ft { get; set; } = null!;
  public string Fm { get; set; } = null!;
  public int Ec { get; set; }
  public int Z { get; set; }
  public string IT { get; set; } = null!;
  public float Erwltp { get; set; }
  public float Ber { get; set; }
  public DateOnly DoFr { get; set; }
  public string SchemeYear { get; set; } = null!;
  public string Postcode { get; set; } = null!;
  public string? Spvc { get; set; }

  public bool Wrm { get; set; }
  public int Mnp { get; set; }
  public string Rlce { get; set; } = null!;
  public int Fa { get; set; }
  public int? MM { get; set; }
  public string Trrc { get; set; } = null!;
}

public class GetVehiclesByManufacturerIdQueryDto
{
  public IEnumerable<VehicleListElementDto> Vehicles { get; set; } = null!;
  public int TotalCount { get; set; }
}