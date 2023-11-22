using System.Globalization;
using CsvHelper.Configuration;

namespace Zev.Services.ComplianceCalculationService.Handler;

public class CsvHelper
{
    public static CsvConfiguration GetCsvConfig() =>
        new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        };
}