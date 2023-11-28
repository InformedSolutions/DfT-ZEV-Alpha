using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class InternalManufacturerActivityConfiguration : IEntityTypeConfiguration<InternalManufacturerActivity>
{
    public void Configure(EntityTypeBuilder<InternalManufacturerActivity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}