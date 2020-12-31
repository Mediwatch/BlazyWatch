using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations
{
    public partial class MigrationAddOrderControllercs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderInfo",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    invoiceId = table.Column<string>(nullable: true),
                    userId = table.Column<string>(nullable: true),
                    formationId = table.Column<string>(nullable: true),
                    createAt = table.Column<DateTime>(nullable: false),
                    billingAdress = table.Column<string>(nullable: true),
                    currency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfo", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInfo");
        }
    }
}
