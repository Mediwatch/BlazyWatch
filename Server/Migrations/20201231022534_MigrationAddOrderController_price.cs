using Microsoft.EntityFrameworkCore.Migrations;

namespace Mediwatch.Server.Migrations
{
    public partial class MigrationAddOrderController_price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "price",
                table: "OrderInfo",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "OrderInfo");
        }
    }
}
