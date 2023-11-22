using FluentValidation;
using FluentValidation.Validators;

namespace Zev.Core.Domain.Vehicles.Validation.Validators;

public class VinValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string vin)
    {
        if (vin.Length != 17)
            return false;
        var index = 0;
        var checkDigit = 0;
        var checkSum = 0;
        var weight = 0;
        foreach (var c in vin.ToCharArray())
        {
            index++;
            var character = c.ToString().ToLower();
            var result = CalculateResult(c);

            if (index >= 1 && index <= 7 || index == 9)
                weight = 9 - index;
            else if (index == 8)
                weight = 10;
            else if (index >= 10 && index <= 17)
                weight = 19 - index;
            if (index == 9)
                checkDigit = character == "x" ? 10 : result;
            checkSum += result * weight;
        }

        return checkSum % 11 == checkDigit;
    }

    public override string Name
        => "VinValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
        => "{PropertyName} must be a valid VIN number.";

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