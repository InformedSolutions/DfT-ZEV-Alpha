using CsvHelper.Configuration;
using Zev.Services.ComplianceCalculationService.Handler.DTO;

namespace Zev.Services.ComplianceCalculationService.Handler;

public sealed class RawVehicleCsvMap : ClassMap<RawVehicleDTO>
{
    public RawVehicleCsvMap()
    {
        Map(m => m.Vin).Name("vin");
        Map(m => m.Vfn).Name("vfn");
        Map(m => m.Mh).Name("Mh");
        Map(m => m.Man).Name("Man");
        Map(m => m.MMS).Name("MMS");
        Map(m => m.TAN).Name("TAN");
        Map(m => m.T).Name("T");
        Map(m => m.Va).Name("Va");
        Map(m => m.Ve).Name("Ve");
        Map(m => m.Mk).Name("Mk");
        Map(m => m.Cn).Name("Cn");
        Map(m => m.Ct).Name("Ct");
        Map(m => m.Cr).Name("Cr");
        Map(m => m.M).Name("M");
        Map(m => m.MT).Name("MT");
        Map(m => m.MRVL).Name("MRVL");
        Map(m => m.Ewltp).Name("Ewltp");
        Map(m => m.TPMLM).Name("TPMLM");
        Map(m => m.W).Name("W");
        Map(m => m.At1).Name("At1");
        Map(m => m.At2).Name("At2");
        Map(m => m.Ft).Name("Ft");
        Map(m => m.Fm).Name("Fm");
        Map(m => m.Ec).Name("Ec");
        Map(m => m.Z).Name("Z");
        Map(m => m.IT).Name("IT");
        Map(m => m.Erwltp).Name("Erwltp");
        Map(m => m.Ber).Name("ber");
        Map(m => m.DoFr).Name("dofr");
        Map(m => m.SchemeYear).Name("scheme_year");
        Map(m => m.Postcode).Name("postcode");
        Map(m => m.Spvc).Name("spvc");
        Map(m => m.Wrm).Name("wrm");
        Map(m => m.Mnp).Name("mnp");
        Map(m => m.Rlce).Name("rlce");
        Map(m => m.Fa).Name("fa");
        Map(m => m.Trrc).Name("trrc");
        Map(m => m.RegisteredInNation).Name("registered_in_nation");
    }
}