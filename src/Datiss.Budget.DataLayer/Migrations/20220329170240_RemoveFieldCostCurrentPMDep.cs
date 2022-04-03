using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class RemoveFieldCostCurrentPMDep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityType",
                table: "CostCurrentPMDeps");

            migrationBuilder.DropColumn(
                name: "RecordType",
                table: "CostCurrentPMDeps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityType",
                table: "CostCurrentPMDeps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RecordType",
                table: "CostCurrentPMDeps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
