using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zev.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BaseVehicleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Vin = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: false),
                    Vfn = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Mh = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Man = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MMS = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: false),
                    TAN = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    T = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Va = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Ve = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Mk = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Cn = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Ct = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Cr = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    M = table.Column<int>(type: "integer", nullable: false),
                    MT = table.Column<int>(type: "integer", nullable: false),
                    MRVL = table.Column<int>(type: "integer", nullable: false),
                    Ewltp = table.Column<int>(type: "integer", nullable: false),
                    TPMLM = table.Column<int>(type: "integer", nullable: false),
                    W = table.Column<int>(type: "integer", nullable: false),
                    At1 = table.Column<int>(type: "integer", nullable: false),
                    At2 = table.Column<int>(type: "integer", nullable: false),
                    Ft = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Fm = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Ec = table.Column<int>(type: "integer", nullable: false),
                    Z = table.Column<int>(type: "integer", nullable: false),
                    IT = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Erwltp = table.Column<float>(type: "numeric(18,2)", nullable: false),
                    Ber = table.Column<float>(type: "numeric(18,2)", nullable: false),
                    DoFr = table.Column<DateOnly>(type: "date", nullable: false),
                    SchemeYear = table.Column<string>(type: "text", nullable: false),
                    Postcode = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Spvc = table.Column<string>(type: "text", nullable: false),
                    Wrm = table.Column<bool>(type: "boolean", nullable: false),
                    Mnp = table.Column<int>(type: "integer", nullable: false),
                    Rlce = table.Column<string>(type: "text", nullable: false),
                    Fa = table.Column<int>(type: "integer", nullable: false),
                    Trrc = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    RegisteredInNation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Vin);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
