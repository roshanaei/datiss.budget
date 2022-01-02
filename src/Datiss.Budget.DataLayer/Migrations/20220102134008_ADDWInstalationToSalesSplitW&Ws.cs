using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ADDWInstalationToSalesSplitWWs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WsInstallationCosts",
                table: "SalesSplitWs_Y",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WInstallationCosts",
                table: "SalesSplitW_Y",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WsInstallationCosts",
                table: "SalesSplitWs_Y");

            migrationBuilder.DropColumn(
                name: "WInstallationCosts",
                table: "SalesSplitW_Y");
        }
    }
}
