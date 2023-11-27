using FluentValidation;
using DfT.ZEV.Core.Domain.Vehicles.Validation.Validators;
using DfT.ZEV.Services.ComplianceCalculation.Handler.DTO;

namespace DfT.ZEV.Services.ComplianceCalculation.Handler.Validation;

public class RawVehicleDtoValidator : AbstractValidator<RawVehicleDTO>
{
    public RawVehicleDtoValidator()
    {
        RuleFor(x => x.Vin).SetAsyncValidator(new VinValidator<RawVehicleDTO>());
    }
}