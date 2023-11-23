using FluentValidation;
using Zev.Core.Domain.Vehicles.Validation.Validators;
using Zev.Services.ComplianceCalculation.Handler.DTO;

namespace Zev.Services.ComplianceCalculation.Handler.Validation;

public class RawVehicleDTOValidator : AbstractValidator<RawVehicleDTO>
{
    public RawVehicleDTOValidator()
    {
        RuleFor(x => x.Vin).SetAsyncValidator(new VinValidator<RawVehicleDTO>());
    }
}