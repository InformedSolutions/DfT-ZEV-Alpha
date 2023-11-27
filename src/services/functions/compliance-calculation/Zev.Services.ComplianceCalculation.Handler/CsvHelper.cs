using System.Globalization;
using CsvHelper.Configuration;

namespace Zev.Services.ComplianceCalculation.Handler;

public class CsvHelper
{
    public static CsvConfiguration GetCsvConfig()
    {
        return new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim
        };
    }
}