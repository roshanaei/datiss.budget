using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddAverageCapacityToSalesSplits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacity",
                table: "SalesSplitWs_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacity",
                table: "SalesSplitW_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageCapacity",
                table: "SalesSplitWs_Y");

            migrationBuilder.DropColumn(
                name: "AverageCapacity",
                table: "SalesSplitW_Y");
        }
    }
}
