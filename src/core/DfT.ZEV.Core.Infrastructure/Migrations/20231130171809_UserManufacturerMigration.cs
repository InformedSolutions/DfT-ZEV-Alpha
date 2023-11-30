using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.ZEV.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserManufacturerMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionUser");

            migrationBuilder.CreateTable(
                name: "UserManufacturerBridge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManufacturerBridge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserManufacturerBridge_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserManufacturerBridge_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionUserManufacturerBridge",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserManufacturerBridgesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUserManufacturerBridge", x => new { x.PermissionsId, x.UserManufacturerBridgesId });
                    table.ForeignKey(
                        name: "FK_PermissionUserManufacturerBridge_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionUserManufacturerBridge_UserManufacturerBridge_Use~",
                        column: x => x.UserManufacturerBridgesId,
                        principalTable: "UserManufacturerBridge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUserManufacturerBridge_UserManufacturerBridgesId",
                table: "PermissionUserManufacturerBridge",
                column: "UserManufacturerBridgesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManufacturerBridge_ManufacturerId",
                table: "UserManufacturerBridge",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserManufacturerBridge_UserId",
                table: "UserManufacturerBridge",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionUserManufacturerBridge");

            migrationBuilder.DropTable(
                name: "UserManufacturerBridge");

            migrationBuilder.CreateTable(
                name: "PermissionUser",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionUser", x => new { x.PermissionsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PermissionUser_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionUser_UsersId",
                table: "PermissionUser",
                column: "UsersId");
        }
    }
}
