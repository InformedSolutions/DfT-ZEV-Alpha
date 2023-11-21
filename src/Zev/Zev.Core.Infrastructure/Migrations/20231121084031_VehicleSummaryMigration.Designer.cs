﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Zev.Core.Infrastructure.Persistence;

#nullable disable

namespace Zev.Core.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231121084031_VehicleSummaryMigration")]
    partial class VehicleSummaryMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Zev.Core.Domain.Vehicles.Vehicle", b =>
                {
                    b.Property<string>("Vin")
                        .HasMaxLength(17)
                        .HasColumnType("character varying(17)");

                    b.Property<int>("At1")
                        .HasColumnType("integer");

                    b.Property<int>("At2")
                        .HasColumnType("integer");

                    b.Property<float>("Ber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Cn")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("Cr")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Ct")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("character varying(2)");

                    b.Property<DateOnly>("DoFr")
                        .HasColumnType("date");

                    b.Property<int>("Ec")
                        .HasColumnType("integer");

                    b.Property<float>("Erwltp")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Ewltp")
                        .HasColumnType("integer");

                    b.Property<int>("Fa")
                        .HasColumnType("integer");

                    b.Property<string>("Fm")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("Ft")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("IT")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("M")
                        .HasColumnType("integer");

                    b.Property<int?>("MM")
                        .HasColumnType("integer");

                    b.Property<string>("MMS")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.Property<int?>("MRVL")
                        .HasColumnType("integer");

                    b.Property<int>("MT")
                        .HasColumnType("integer");

                    b.Property<string>("Man")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Mh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Mk")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<int>("Mnp")
                        .HasColumnType("integer");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("RegisteredInNation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Rlce")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SchemeYear")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Spvc")
                        .HasColumnType("text");

                    b.Property<string>("T")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("TAN")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<int>("TPMLM")
                        .HasColumnType("integer");

                    b.Property<string>("Trrc")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("Va")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("Ve")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("Vfn")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<int>("W")
                        .HasColumnType("integer");

                    b.Property<bool>("Wrm")
                        .HasColumnType("boolean");

                    b.Property<int>("Z")
                        .HasColumnType("integer");

                    b.HasKey("Vin");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Zev.Core.Domain.Vehicles.VehicleSummary", b =>
                {
                    b.Property<string>("Vin")
                        .HasColumnType("character varying(17)");

                    b.Property<bool?>("Co2Applicable")
                        .HasColumnType("boolean");

                    b.Property<bool?>("IncompleteMsv")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Rrr")
                        .HasColumnType("boolean");

                    b.Property<string>("VehicleScheme")
                        .HasColumnType("text");

                    b.Property<bool?>("Wca")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Wcs")
                        .HasColumnType("boolean");

                    b.Property<bool?>("Zev")
                        .HasColumnType("boolean");

                    b.Property<bool?>("ZevApplicable")
                        .HasColumnType("boolean");

                    b.Property<bool?>("msv")
                        .HasColumnType("boolean");

                    b.HasKey("Vin");

                    b.ToTable("VehicleSummary");
                });

            modelBuilder.Entity("Zev.Core.Domain.Vehicles.VehicleSummary", b =>
                {
                    b.HasOne("Zev.Core.Domain.Vehicles.Vehicle", "Vehicle")
                        .WithOne("Summary")
                        .HasForeignKey("Zev.Core.Domain.Vehicles.VehicleSummary", "Vin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Zev.Core.Domain.Vehicles.Vehicle", b =>
                {
                    b.Navigation("Summary")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
