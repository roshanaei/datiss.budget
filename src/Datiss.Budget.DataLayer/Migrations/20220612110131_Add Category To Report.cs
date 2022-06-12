using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCategoryToReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CategoryTypeId",
                table: "Reports",
                column: "CategoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports",
                column: "CategoryTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CategoryTypeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CategoryTypeId",
                table: "Reports");
        }
    }
}
