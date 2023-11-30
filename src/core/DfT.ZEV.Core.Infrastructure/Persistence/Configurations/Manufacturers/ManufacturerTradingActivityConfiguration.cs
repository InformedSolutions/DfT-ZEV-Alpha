using DfT.ZEV.Core.Domain.Manufacturers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Manufacturers;

internal sealed class ManufacturerTradingActivityConfiguration : IEntityTypeConfiguration<ManufacturerTradingActivity>
{
    public void Configure(EntityTypeBuilder<ManufacturerTradingActivity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}