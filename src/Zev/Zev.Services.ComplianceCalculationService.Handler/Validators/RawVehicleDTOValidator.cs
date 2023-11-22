using FluentValidation;
using Zev.Core.Domain.Vehicles.Validation.Validators;
using Zev.Services.ComplianceCalculationService.Handler.DTO;

namespace Zev.Services.ComplianceCalculationService.Handler.Validators;

public class RawVehicleDTOValidator : AbstractValidator<RawVehicleDTO>
{
    public RawVehicleDTOValidator()
    {
        RuleFor(x => x.Vin).SetAsyncValidator(new VinValidator<RawVehicleDTO>());
    }
}