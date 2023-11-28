using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Permissions);
    }
}