using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations.DbContextMediwatchMigrations
{
    public partial class CreateMediwatchDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Formation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idCompagny = table.Column<int>(nullable: false),
                    formationName = table.Column<string>(nullable: true),
                    createdAt = table.Column<DateTime>(nullable: false),
                    formationTime = table.Column<DateTime>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    talker = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    free = table.Column<bool>(nullable: false),
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
                    createdAt = table.Column<DateTime>(nullable: false),
                    formationid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicant_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_applicant_sessions_Formation_formationid",
                        column: x => x.formationid,
                        principalTable: "Formation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tag_name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    formationid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.id);
                    table.ForeignKey(
                        name: "FK_tag_Formation_formationid",
                        column: x => x.formationid,
                        principalTable: "Formation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_applicant_sessions_formationid",
                table: "applicant_sessions",
                column: "formationid");

            migrationBuilder.CreateIndex(
                name: "IX_Formation_compagnyid",
                table: "Formation",
                column: "compagnyid");

            migrationBuilder.CreateIndex(
                name: "IX_tag_formationid",
                table: "tag",
                column: "formationid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicant_sessions");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "Formation");

            migrationBuilder.DropTable(
                name: "Compagny");
        }
    }
}
