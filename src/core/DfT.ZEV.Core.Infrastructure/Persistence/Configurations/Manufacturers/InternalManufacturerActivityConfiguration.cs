using DfT.ZEV.Core.Domain.Manufacturers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Manufacturers;

internal sealed class InternalManufacturerActivityConfiguration : IEntityTypeConfiguration<InternalManufacturerActivity>
{
    public void Configure(EntityTypeBuilder<InternalManufacturerActivity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}