using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zev.Core.Domain.Vehicles;

namespace Zev.Core.Infrastructure.Persistence.Configurations.Vehicles;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(x => x.Vin);

            builder.Property(x => x.VehicleDetails)
                .HasMaxLength(255);

            builder.Property(x => x.Co2Value)
                .IsRequired();

            builder.Property(x => x.BonusCreditValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Vin)
                .HasMaxLength(17);

            builder.Property(x => x.Vfn)
                .HasMaxLength(25);

            builder.Property(x => x.Mh)
                .HasMaxLength(50);

            builder.Property(x => x.Man)
                .HasMaxLength(50);

            builder.Property(x => x.MMS)
                .HasMaxLength(125);

            builder.Property(x => x.TAN)
                .HasMaxLength(120);

            builder.Property(x => x.T)
                .HasMaxLength(120);

            builder.Property(x => x.Va)
                .HasMaxLength(120);

            builder.Property(x => x.Ve)
                .HasMaxLength(120);

            builder.Property(x => x.Mk)
                .HasMaxLength(120);

            builder.Property(x => x.Cn)
                .HasMaxLength(120);

            builder.Property(x => x.Ct)
                .HasMaxLength(2);

            builder.Property(x => x.Cr)
                .HasMaxLength(255);
            
            builder.Property(x => x.Ft)
                .HasMaxLength(120);

            builder.Property(x => x.Fm)
                .HasMaxLength(1);

            builder.Property(x => x.IT)
                .HasMaxLength(255);

            builder.Property(x => x.Erwltp)
                .HasColumnType("decimal(18,2)");
            
            builder.Property(x => x.Ber)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Postcode)
                .HasMaxLength(8);

            

            builder.Property(x => x.Trrc)
                .HasMaxLength(1);

          
        }
    }