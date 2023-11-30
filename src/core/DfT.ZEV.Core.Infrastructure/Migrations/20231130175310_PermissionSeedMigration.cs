using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DfT.ZEV.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermissionSeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { new Guid("5f3f4351-b93f-4fab-bd7f-9a9862c2668a"), "Permission5" },
                    { new Guid("8d663a11-6208-4929-8fcf-f54e1d5bcc14"), "Permission2" },
                    { new Guid("afbfd357-dca4-47b8-8b5c-c7b617bac30d"), "Permission6" },
                    { new Guid("c3097496-533b-4372-8086-c64e70b5c90e"), "Permission3" },
                    { new Guid("d9485840-e3de-4313-9553-73c6d09c4721"), "Permission4" },
                    { new Guid("eb45c32f-5f2e-4a5f-bfbb-5d2e1d786b3a"), "Permission1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("5f3f4351-b93f-4fab-bd7f-9a9862c2668a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("8d663a11-6208-4929-8fcf-f54e1d5bcc14"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("afbfd357-dca4-47b8-8b5c-c7b617bac30d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c3097496-533b-4372-8086-c64e70b5c90e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d9485840-e3de-4313-9553-73c6d09c4721"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("eb45c32f-5f2e-4a5f-bfbb-5d2e1d786b3a"));
        }
    }
}
