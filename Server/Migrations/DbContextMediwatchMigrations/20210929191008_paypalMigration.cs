using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations.DbContextMediwatchMigrations
{
    public partial class paypalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityCurrent",
                table: "Formation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityMax",
                table: "Formation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "idPayPal",
                table: "Applicant_session",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityCurrent",
                table: "Formation");

            migrationBuilder.DropColumn(
                name: "QuantityMax",
                table: "Formation");

            migrationBuilder.DropColumn(
                name: "idPayPal",
                table: "Applicant_session");
        }
    }
}
