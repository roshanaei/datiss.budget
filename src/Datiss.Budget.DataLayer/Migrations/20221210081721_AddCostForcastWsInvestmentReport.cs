using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastWsInvestmentReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastWsInvestmentReport",
                columns: table => new
                {
                    CFWsIRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    SectionTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount1 = table.Column<int>(type: "int", nullable: false),
                    Cost1 = table.Column<long>(type: "bigint", nullable: false),
                    Amount2 = table.Column<int>(type: "int", nullable: false),
                    Cost2 = table.Column<long>(type: "bigint", nullable: false),
                    Amount3 = table.Column<int>(type: "int", nullable: false),
                    Cost3 = table.Column<long>(type: "bigint", nullable: false),
                    Amount4 = table.Column<int>(type: "int", nullable: false),
                    Cost4 = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastWsInvestmentReport", x => x.CFWsIRId);
                    table.ForeignKey(
                        name: "FK_CostForcastWsInvestmentReport_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWsInvestmentReport_Constants_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWsInvestmentReport_Constants_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWsInvestmentReport_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWsInvestmentReport_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWsInvestmentReport_CostCenterTypeId",
                table: "CostForcastWsInvestmentReport",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWsInvestmentReport_OrganizationId",
                table: "CostForcastWsInvestmentReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWsInvestmentReport_SectionTypeId",
                table: "CostForcastWsInvestmentReport",
                column: "SectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWsInvestmentReport_UnitTypeId",
                table: "CostForcastWsInvestmentReport",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWsInvestmentReport_YearId",
                table: "CostForcastWsInvestmentReport",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastWsInvestmentReport");
        }
    }
}
