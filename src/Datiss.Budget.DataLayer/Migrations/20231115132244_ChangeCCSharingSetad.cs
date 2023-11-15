using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeCCSharingSetad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeCurrentW",
                table: "CostCurrentSharingSetad");

            migrationBuilder.DropColumn(
                name: "IncomeCurrentWs",
                table: "CostCurrentSharingSetad");

            migrationBuilder.DropColumn(
                name: "IncomeForcast",
                table: "CostCurrentSharingSetad");

            migrationBuilder.DropColumn(
                name: "SPSHahrdari",
                table: "CostCurrentSharingSetad");

            migrationBuilder.DropColumn(
                name: "WUnit",
                table: "CostCurrentSharingSetad");

            migrationBuilder.DropColumn(
                name: "WsUnit",
                table: "CostCurrentSharingSetad");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IncomeCurrentW",
                table: "CostCurrentSharingSetad",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IncomeCurrentWs",
                table: "CostCurrentSharingSetad",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "IncomeForcast",
                table: "CostCurrentSharingSetad",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "SPSHahrdari",
                table: "CostCurrentSharingSetad",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WUnit",
                table: "CostCurrentSharingSetad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WsUnit",
                table: "CostCurrentSharingSetad",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
