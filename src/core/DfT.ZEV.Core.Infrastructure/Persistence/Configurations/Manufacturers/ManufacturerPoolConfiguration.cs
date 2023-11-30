using DfT.ZEV.Core.Domain.Manufacturers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DfT.ZEV.Core.Infrastructure.Persistence.Configurations.Manufacturers;

internal sealed class ManufacturerPoolConfiguration : IEntityTypeConfiguration<ManufacturerPool>
{
    public void Configure(EntityTypeBuilder<ManufacturerPool> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired();
        
        builder.HasOne(x => x.PrimaryContact)
            .WithMany(x => x.ManufacturerPools)
            .HasForeignKey(x => x.PrimaryContactId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.PrimaryContact)
            .AutoInclude();
    }
}