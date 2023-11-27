using System.ComponentModel.DataAnnotations;
using DfT.ZEV.Common.Validation;

namespace DfT.ZEV.Common.Attributes;

[AttributeUsage(
    AttributeTargets.Property |
    AttributeTargets.Field |
    AttributeTargets.Parameter,
    AllowMultiple = false)]
public class CustomEmailAddressAttribute : DataTypeAttribute
{
    public CustomEmailAddressAttribute()
        : base(DataType.EmailAddress)
    {
        ErrorMessage = "Enter your email address in a valid format";
    }

#nullable enable
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not string valueAsString)
        {
            return false;
        }

        return EmailValidator.IsValid(valueAsString);
    }
#nullable disable
}
