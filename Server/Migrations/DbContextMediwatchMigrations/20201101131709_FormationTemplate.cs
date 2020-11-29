using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations.DbContextMediwatchMigrations
{
<<<<<<< HEAD:Server/Migrations/DbContextMediwatchMigrations/20201129151831_innitialCreate.cs
    public partial class innitialCreate : Migration
=======
    public partial class FormationTemplate : Migration
>>>>>>> cb6001234b4279f6115e7bb4cdef324e54822de6:Server/Migrations/DbContextMediwatchMigrations/20201101131709_FormationTemplate.cs
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applicant_sessions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idFormation = table.Column<int>(nullable: false),
                    idUser = table.Column<int>(nullable: false),
                    confirmed = table.Column<bool>(nullable: false),
                    payed = table.Column<bool>(nullable: false),
                    createdAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicant_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Compagny",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    compagnyName = table.Column<string>(nullable: true),
                    countryCode = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compagny", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tag_name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Formation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    OrganizationName = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Former = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Target = table.Column<string>(nullable: true),
                    compagnyid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formation", x => x.id);
                    table.ForeignKey(
                        name: "FK_Formation_Compagny_compagnyid",
                        column: x => x.compagnyid,
                        principalTable: "Compagny",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Formation_compagnyid",
                table: "Formation",
                column: "compagnyid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicant_sessions");

            migrationBuilder.DropTable(
                name: "Formation");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "Compagny");
        }
    }
}
