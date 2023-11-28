using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class RolesBridgeTableConfiguration : IEntityTypeConfiguration<RolesBridge>
{
    public void Configure(EntityTypeBuilder<RolesBridge> builder)
    {
        builder.HasNoKey();

        builder.HasOne(x => x.Manufacturer)
            .WithMany(x => x.RolesBridgeTable)
            .HasForeignKey(x => x.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(x => x.Manufacturer)
            .AutoInclude();
        
        builder.HasOne(x => x.Account)
            .WithMany(x => x.RolesBridgeTable)
            .HasForeignKey(x => x.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(x => x.Account)
            .AutoInclude();
        
        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolesBridgeTable)
            .HasForeignKey(x => x.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Navigation(x => x.Account)
            .AutoInclude();
    }
}