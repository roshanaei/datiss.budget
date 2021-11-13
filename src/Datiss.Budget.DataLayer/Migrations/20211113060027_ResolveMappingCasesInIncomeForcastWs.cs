using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ResolveMappingCasesInIncomeForcastWs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeForcastWs_Organizations_UserTypeId",
                table: "IncomeForcastWs");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastWs_OrganizationId",
                table: "IncomeForcastWs",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeForcastWs_Organizations_OrganizationId",
                table: "IncomeForcastWs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeForcastWs_Organizations_OrganizationId",
                table: "IncomeForcastWs");

            migrationBuilder.DropIndex(
                name: "IX_IncomeForcastWs_OrganizationId",
                table: "IncomeForcastWs");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeForcastWs_Organizations_UserTypeId",
                table: "IncomeForcastWs",
                column: "UserTypeId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
