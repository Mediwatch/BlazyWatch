using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations.DbContextMediwatchMigrations
{
    public partial class innitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicant_session",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    idFormation = table.Column<float>(type: "REAL", nullable: false),
                    idUser = table.Column<Guid>(type: "TEXT", nullable: false),
                    confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    payed = table.Column<bool>(type: "INTEGER", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicant_session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BlazingArticles",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    PreviewImageURL = table.Column<string>(type: "TEXT", nullable: true),
                    PreviewTitle = table.Column<string>(type: "TEXT", nullable: true),
                    PreviewParagraph = table.Column<string>(type: "TEXT", nullable: true),
                    _tags = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlazingArticles", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Compagny",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    compagnyName = table.Column<string>(type: "TEXT", nullable: true),
                    countryCode = table.Column<string>(type: "TEXT", nullable: true),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compagny", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderInfo",
                columns: table => new
                {
                    id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    invoiceId = table.Column<string>(type: "TEXT", nullable: true),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    formationId = table.Column<string>(type: "TEXT", nullable: true),
                    createAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    billingAdress = table.Column<string>(type: "TEXT", nullable: true),
                    currency = table.Column<string>(type: "TEXT", nullable: true),
                    price = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tag_name = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Formation",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Contact = table.Column<string>(type: "TEXT", nullable: true),
                    OrganizationName = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Former = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Target = table.Column<string>(type: "TEXT", nullable: true),
                    ArticleID = table.Column<string>(type: "TEXT", nullable: true),
                    compagnyid = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "Applicant_session");

            migrationBuilder.DropTable(
                name: "BlazingArticles");

            migrationBuilder.DropTable(
                name: "Formation");

            migrationBuilder.DropTable(
                name: "OrderInfo");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Compagny");
        }
    }
}
