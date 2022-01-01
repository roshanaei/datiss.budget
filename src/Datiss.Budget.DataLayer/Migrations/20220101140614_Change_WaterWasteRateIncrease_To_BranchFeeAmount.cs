using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class Change_WaterWasteRateIncrease_To_BranchFeeAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterWasteBranchingAmount_FinanceYears_YearId",
                table: "WaterWasteBranchingAmount");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterWasteBranchingAmount_Organizations_OrganizationId",
                table: "WaterWasteBranchingAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaterWasteBranchingAmount",
                table: "WaterWasteBranchingAmount");

            migrationBuilder.RenameTable(
                name: "WaterWasteBranchingAmount",
                newName: "BranchFeeAmount");

            migrationBuilder.RenameIndex(
                name: "IX_WaterWasteBranchingAmount_YearId",
                table: "BranchFeeAmount",
                newName: "IX_BranchFeeAmount_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_WaterWasteBranchingAmount_OrganizationId",
                table: "BranchFeeAmount",
                newName: "IX_BranchFeeAmount_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchFeeAmount",
                table: "BranchFeeAmount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchFeeAmount_FinanceYears_YearId",
                table: "BranchFeeAmount",
                column: "YearId",
                principalTable: "FinanceYears",
                principalColumn: "FinanceYearId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BranchFeeAmount_Organizations_OrganizationId",
                table: "BranchFeeAmount",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchFeeAmount_FinanceYears_YearId",
                table: "BranchFeeAmount");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchFeeAmount_Organizations_OrganizationId",
                table: "BranchFeeAmount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchFeeAmount",
                table: "BranchFeeAmount");

            migrationBuilder.RenameTable(
                name: "BranchFeeAmount",
                newName: "WaterWasteBranchingAmount");

            migrationBuilder.RenameIndex(
                name: "IX_BranchFeeAmount_YearId",
                table: "WaterWasteBranchingAmount",
                newName: "IX_WaterWasteBranchingAmount_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_BranchFeeAmount_OrganizationId",
                table: "WaterWasteBranchingAmount",
                newName: "IX_WaterWasteBranchingAmount_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaterWasteBranchingAmount",
                table: "WaterWasteBranchingAmount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterWasteBranchingAmount_FinanceYears_YearId",
                table: "WaterWasteBranchingAmount",
                column: "YearId",
                principalTable: "FinanceYears",
                principalColumn: "FinanceYearId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterWasteBranchingAmount_Organizations_OrganizationId",
                table: "WaterWasteBranchingAmount",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
