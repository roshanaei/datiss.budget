using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddTotalBudgetWsReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TotalBudgetWsReport",
                columns: table => new
                {
                    TBWsRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SectionTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    FunctionalYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    FunctionalBasicYear = table.Column<long>(type: "bigint", nullable: false),
                    ApproveYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    ForcastY = table.Column<long>(type: "bigint", nullable: false),
                    ForcastFunctionalPercent = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ForcastBudgetPercent = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalBudgetWsReport", x => x.TBWsRId);
                    table.ForeignKey(
                        name: "FK_TotalBudgetWsReport_Constants_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TotalBudgetWsReport_Constants_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TotalBudgetWsReport_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TotalBudgetWsReport_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalBudgetWsReport_OrganizationId",
                table: "TotalBudgetWsReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalBudgetWsReport_SectionTypeId",
                table: "TotalBudgetWsReport",
                column: "SectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalBudgetWsReport_UnitTypeId",
                table: "TotalBudgetWsReport",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalBudgetWsReport_YearId",
                table: "TotalBudgetWsReport",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalBudgetWsReport");
        }
    }
}
