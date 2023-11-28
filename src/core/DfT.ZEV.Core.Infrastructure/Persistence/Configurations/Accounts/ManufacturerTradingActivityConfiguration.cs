using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class ManufacturerTradingActivityConfiguration : IEntityTypeConfiguration<ManufacturerTradingActivity>
{
    public void Configure(EntityTypeBuilder<ManufacturerTradingActivity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}