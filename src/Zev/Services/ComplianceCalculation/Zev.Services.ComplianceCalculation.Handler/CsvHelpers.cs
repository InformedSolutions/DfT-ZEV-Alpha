using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Zev.Services.ComplianceCalculation.Handler.Maps;
using Zev.Services.ComplianceCalculationService.Handler.Maps;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class CsvHelpers
{
    public static CsvConfiguration GetCsvConfig() =>
        new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        };

    public static CsvContext ConfigureContext(CsvContext context)
    {
        context.RegisterClassMap<RawVehicleCsvMap>();
        context.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add(string.Empty);
        context.TypeConverterOptionsCache.GetOptions<int>().NullValues.Add(string.Empty);
        context.TypeConverterOptionsCache.GetOptions<float>().NullValues.Add(string.Empty);
        var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy", "dd-MM-yyyy", "yyyy-MM-dd" } };
        context.TypeConverterOptionsCache.AddOptions<DateOnly>(options);
        return context;
    }
    
    
}