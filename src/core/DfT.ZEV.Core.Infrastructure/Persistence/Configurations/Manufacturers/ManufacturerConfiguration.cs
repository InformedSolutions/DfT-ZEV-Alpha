using DfT.ZEV.Core.Domain.Manufacturers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Manufacturers;

internal sealed class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired();
        
        builder.Property(x => x.PoolMemberId)
            .IsRequired();
        
        builder.Property(x => x.Co2Target)
            .IsRequired();
        
        builder.Property(x => x.DerogationStatus)
            .IsRequired();
        
        builder.Navigation(x => x.RolesBridgeTable)
            .AutoInclude();
    }
}