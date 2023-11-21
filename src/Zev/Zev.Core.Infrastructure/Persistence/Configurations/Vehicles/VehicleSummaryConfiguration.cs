using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Persistence.Configurations.Vehicles;

public class VehicleSummaryConfiguration : IEntityTypeConfiguration<VehicleSummary>
{
    public void Configure(EntityTypeBuilder<VehicleSummary> builder)
    {
        builder.HasKey(x => x.Vin);
        
    }
}