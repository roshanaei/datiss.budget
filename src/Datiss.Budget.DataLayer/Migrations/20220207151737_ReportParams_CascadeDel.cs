using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ReportParams_CascadeDel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportParams_Reports_ReportId",
                table: "ReportParams");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportParams_Reports_ReportId",
                table: "ReportParams",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportParams_Reports_ReportId",
                table: "ReportParams");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportParams_Reports_ReportId",
                table: "ReportParams",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
