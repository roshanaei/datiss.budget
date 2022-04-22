using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CreateBudgetSourceReportMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetSourceReports",
                columns: table => new
                {
                    BudgetSourceReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SectionTypeId = table.Column<int>(type: "int", nullable: false),
                    FunctionalBasicYear = table.Column<long>(type: "bigint", nullable: false),
                    FunctionalYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    ApproveYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    ForcastY = table.Column<long>(type: "bigint", nullable: false),
                    ReceiptPercent = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Fee = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_BudgetSourceReports", x => x.BudgetSourceReportId);
                    table.ForeignKey(
                        name: "FK_BudgetSourceReports_Constants_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetSourceReports_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetSourceReports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetSourceReports_OrganizationId",
                table: "BudgetSourceReports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetSourceReports_SectionTypeId",
                table: "BudgetSourceReports",
                column: "SectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetSourceReports_YearId",
                table: "BudgetSourceReports",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetSourceReports");
        }
    }
}
