using DfT.ZEV.Core.Domain.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class UserManufacturerBridgeConfiguration : IEntityTypeConfiguration<UserManufacturerBridge>
{
    public void Configure(EntityTypeBuilder<UserManufacturerBridge> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.ManufacturerBridges)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Manufacturer)
            .WithMany(x => x.UserBridges)
            .HasForeignKey(x => x.ManufacturerId);

        builder.Navigation(x => x.Manufacturer)
            .AutoInclude();
        
        builder.HasMany(x => x.Permissions)
            .WithMany(x => x.UserManufacturerBridges);
        
        builder.Navigation(x => x.Permissions)
            .AutoInclude();
    }
}