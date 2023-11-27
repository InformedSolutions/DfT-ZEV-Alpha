using FluentValidation;
using DfT.ZEV.Core.Domain.Vehicles.Validation.Validators;
using Zev.Services.ComplianceCalculation.Handler.DTO;

namespace Zev.Services.ComplianceCalculation.Handler.Validation;

public class RawVehicleDtoValidator : AbstractValidator<RawVehicleDTO>
{
    public RawVehicleDtoValidator()
    {
        RuleFor(x => x.Vin).SetAsyncValidator(new VinValidator<RawVehicleDTO>());
    }
}