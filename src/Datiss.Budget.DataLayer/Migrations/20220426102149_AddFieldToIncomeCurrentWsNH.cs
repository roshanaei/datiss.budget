using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddFieldToIncomeCurrentWsNH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExcessConsumption",
                table: "IncomeCurrentWsNH",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcessConsumption",
                table: "IncomeCurrentWsNH");
        }
    }
}
