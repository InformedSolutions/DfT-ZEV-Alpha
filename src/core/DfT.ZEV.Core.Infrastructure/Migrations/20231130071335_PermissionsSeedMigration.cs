using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DfT.ZEV.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermissionsSeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { new Guid("00261482-1eb5-4ea5-b074-8424561650f5"), "Permission5" },
                    { new Guid("635170b6-928e-4c0a-b18c-afad1b60b86a"), "Permission1" },
                    { new Guid("788bf4a9-c4b3-4f87-a82d-a29551fff15d"), "Permission3" },
                    { new Guid("a8b8b7ca-3458-4cfc-83bf-f178426f4d63"), "Permission6" },
                    { new Guid("efdbd00d-88d8-47eb-920c-d7fd5258ebfa"), "Permission4" },
                    { new Guid("f7de8f99-739d-4b19-b779-13cb1cf1204d"), "Permission2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("00261482-1eb5-4ea5-b074-8424561650f5"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("635170b6-928e-4c0a-b18c-afad1b60b86a"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("788bf4a9-c4b3-4f87-a82d-a29551fff15d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a8b8b7ca-3458-4cfc-83bf-f178426f4d63"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("efdbd00d-88d8-47eb-920c-d7fd5258ebfa"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f7de8f99-739d-4b19-b779-13cb1cf1204d"));
        }
    }
}
