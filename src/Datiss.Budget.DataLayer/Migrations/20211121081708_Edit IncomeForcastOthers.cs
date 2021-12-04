using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class EditIncomeForcastOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OIFCount",
                table: "IncomeForcastOthers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OIFCount",
                table: "IncomeForcastOthers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
