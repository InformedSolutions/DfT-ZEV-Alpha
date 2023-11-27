using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DfT.ZEV.Core.Domain.Vehicles.Models;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Vehicles;

public class VehicleSummaryConfiguration : IEntityTypeConfiguration<VehicleSummary>
{
    public void Configure(EntityTypeBuilder<VehicleSummary> builder)
    {
        builder.HasKey(x => x.Vin);
    }
}