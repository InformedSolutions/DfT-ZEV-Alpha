using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.ZEV.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BaseAccountsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalManufacturerActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ActivityType = table.Column<string>(type: "text", nullable: false),
                    ActionInitiated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalManufacturerActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PoolMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    Co2Target = table.Column<float>(type: "real", nullable: false),
                    DerogationStatus = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturerTradingActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<char>(type: "character(1)", nullable: false),
                    InitiatingManufacturerId = table.Column<Guid>(type: "uuid", nullable: true),
                    AcceptingManufacturerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApplicableScheme = table.Column<string>(type: "text", nullable: false),
                    TradeType = table.Column<string>(type: "text", nullable: false),
                    TradeAmount = table.Column<float>(type: "real", nullable: false),
                    TradeValue = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerTradingActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManufacturerTradingActivities_Manufacturers_AcceptingManufa~",
                        column: x => x.AcceptingManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManufacturerTradingActivities_Manufacturers_InitiatingManuf~",
                        column: x => x.InitiatingManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ManufacturerPools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PrimaryContactId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerPools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManufacturerPools_Users_PrimaryContactId",
                        column: x => x.PrimaryContactId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolesBridges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesBridges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolesBridges_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesBridges_Roles_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolesBridges_Users_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerPools_PrimaryContactId",
                table: "ManufacturerPools",
                column: "PrimaryContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerTradingActivities_AcceptingManufacturerId",
                table: "ManufacturerTradingActivities",
                column: "AcceptingManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerTradingActivities_InitiatingManufacturerId",
                table: "ManufacturerTradingActivities",
                column: "InitiatingManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesBridges_ManufacturerId",
                table: "RolesBridges",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalManufacturerActivities");

            migrationBuilder.DropTable(
                name: "ManufacturerPools");

            migrationBuilder.DropTable(
                name: "ManufacturerTradingActivities");

            migrationBuilder.DropTable(
                name: "RolesBridges");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
