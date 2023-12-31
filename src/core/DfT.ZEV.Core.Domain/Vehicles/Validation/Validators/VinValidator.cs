using FluentValidation;
using FluentValidation.Validators;

namespace DfT.ZEV.Core.Domain.Vehicles.Validation.Validators;

public sealed class VinValidator<T> : AsyncPropertyValidator<T, string>
{
    public override string Name
        => "VinValidator";

    public override Task<bool> IsValidAsync(ValidationContext<T> context, string vin, CancellationToken token)
    {
        if (vin.Length != 17)
            return Task.FromResult(false);
        var index = 0;
        var checkDigit = 0;
        var checkSum = 0;
        var weight = 0;
        foreach (var c in vin.ToCharArray())
        {
            index++;
            var character = c.ToString().ToLower();
            var result = CalculateResult(c);

            weight = index switch
            {
                >= 1 and <= 7 or 9 => 9 - index,
                8 => 10,
                >= 10 and <= 17 => 19 - index,
                _ => weight
            };
            if (index == 9)
                checkDigit = character == "x" ? 10 : result;
            checkSum += result * weight;
        }

        return Task.FromResult(checkSum % 11 == checkDigit);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "{PropertyName} must be a valid VIN number.";
    }

    private static int CalculateResult(char c)
    {
        if (char.IsNumber(c))
            return int.Parse(c.ToString());

        return c.ToString().ToLower() switch
        {
            "a" or "j" => 1,
            "b" or "k" or "s" => 2,
            "c" or "l" or "t" => 3,
            "d" or "m" or "u" => 4,
            "e" or "n" or "v" => 5,
            "f" or "w" => 6,
            "g" or "p" or "x" => 7,
            "h" or "y" => 8,
            "r" or "z" => 9,
            _ => 0
        };
    }
}