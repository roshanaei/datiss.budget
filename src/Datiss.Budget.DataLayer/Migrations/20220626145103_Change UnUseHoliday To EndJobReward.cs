using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeUnUseHolidayToEndJobReward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnUseHolidayCount",
                table: "CostCurrentPersonel",
                newName: "EndJobReward");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndJobReward",
                table: "CostCurrentPersonel",
                newName: "UnUseHolidayCount");
        }
    }
}
