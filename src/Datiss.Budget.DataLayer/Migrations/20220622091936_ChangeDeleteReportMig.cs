using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeDeleteReportMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports",
                column: "CategoryTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Constants_CategoryTypeId",
                table: "Reports",
                column: "CategoryTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
