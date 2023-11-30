﻿// <auto-generated />
using System;
using System.Text.Json;
using DfT.ZEV.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DfT.ZEV.Core.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231130071335_PermissionsSeedMigration")]
    partial class PermissionsSeedMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("635170b6-928e-4c0a-b18c-afad1b60b86a"),
                            PermissionName = "Permission1"
                        },
                        new
                        {
                            Id = new Guid("f7de8f99-739d-4b19-b779-13cb1cf1204d"),
                            PermissionName = "Permission2"
                        },
                        new
                        {
                            Id = new Guid("788bf4a9-c4b3-4f87-a82d-a29551fff15d"),
                            PermissionName = "Permission3"
                        },
                        new
                        {
                            Id = new Guid("efdbd00d-88d8-47eb-920c-d7fd5258ebfa"),
                            PermissionName = "Permission4"
                        },
                        new
                        {
                            Id = new Guid("00261482-1eb5-4ea5-b074-8424561650f5"),
                            PermissionName = "Permission5"
                        },
                        new
                        {
                            Id = new Guid("a8b8b7ca-3458-4cfc-83bf-f178426f4d63"),
                            PermissionName = "Permission6"
                        });
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.RolesBridge", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("RolesBridges");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.InternalManufacturerActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ActionInitiated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ActivityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("InternalManufacturerActivities");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.Manufacturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Co2Target")
                        .HasColumnType("real");

                    b.Property<char>("DerogationStatus")
                        .HasColumnType("character(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PoolMemberId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.ManufacturerPool", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("PrimaryContactId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryContactId");

                    b.ToTable("ManufacturerPools");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.ManufacturerTradingActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AcceptingManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<string>("ApplicableScheme")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("InitiatingManufacturerId")
                        .HasColumnType("uuid");

                    b.Property<char>("Status")
                        .HasColumnType("character(1)");

                    b.Property<float>("TradeAmount")
                        .HasColumnType("real");

                    b.Property<string>("TradeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("TradeValue")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AcceptingManufacturerId");

                    b.HasIndex("InitiatingManufacturerId");

                    b.ToTable("ManufacturerTradingActivities");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Processes.Models.Process", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Finished")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<JsonDocument>("Metadata")
                        .HasColumnType("jsonb");

                    b.Property<JsonDocument>("Result")
                        .HasColumnType("jsonb");

                    b.Property<DateTime?>("Started")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Processes");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Vehicles.Models.Vehicle", b =>
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

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Vehicles.Models.VehicleSummary", b =>
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

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.Property<Guid>("PermissionsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("PermissionsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("PermissionUser");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.RolesBridge", b =>
                {
                    b.HasOne("DfT.ZEV.Core.Domain.Accounts.Models.Role", "Role")
                        .WithMany("RolesBridgeTable")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DfT.ZEV.Core.Domain.Accounts.Models.User", "Account")
                        .WithMany("RolesBridges")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DfT.ZEV.Core.Domain.Manufacturers.Models.Manufacturer", "Manufacturer")
                        .WithMany("RolesBridgeTable")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Manufacturer");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.ManufacturerPool", b =>
                {
                    b.HasOne("DfT.ZEV.Core.Domain.Accounts.Models.User", "PrimaryContact")
                        .WithMany("ManufacturerPools")
                        .HasForeignKey("PrimaryContactId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("PrimaryContact");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.ManufacturerTradingActivity", b =>
                {
                    b.HasOne("DfT.ZEV.Core.Domain.Manufacturers.Models.Manufacturer", "AcceptingManufacturer")
                        .WithMany()
                        .HasForeignKey("AcceptingManufacturerId");

                    b.HasOne("DfT.ZEV.Core.Domain.Manufacturers.Models.Manufacturer", "InitiatingManufacturer")
                        .WithMany()
                        .HasForeignKey("InitiatingManufacturerId");

                    b.Navigation("AcceptingManufacturer");

                    b.Navigation("InitiatingManufacturer");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Vehicles.Models.VehicleSummary", b =>
                {
                    b.HasOne("DfT.ZEV.Core.Domain.Vehicles.Models.Vehicle", "Vehicle")
                        .WithOne("Summary")
                        .HasForeignKey("DfT.ZEV.Core.Domain.Vehicles.Models.VehicleSummary", "Vin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.HasOne("DfT.ZEV.Core.Domain.Accounts.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DfT.ZEV.Core.Domain.Accounts.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.Role", b =>
                {
                    b.Navigation("RolesBridgeTable");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Accounts.Models.User", b =>
                {
                    b.Navigation("ManufacturerPools");

                    b.Navigation("RolesBridges");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Manufacturers.Models.Manufacturer", b =>
                {
                    b.Navigation("RolesBridgeTable");
                });

            modelBuilder.Entity("DfT.ZEV.Core.Domain.Vehicles.Models.Vehicle", b =>
                {
                    b.Navigation("Summary")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
