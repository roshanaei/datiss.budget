using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CostForcastWIReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastWInvestmentReport",
                columns: table => new
                {
                    CFWIRId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_CostForcastWInvestmentReport", x => x.CFWIRId);
                    table.ForeignKey(
                        name: "FK_CostForcastWInvestmentReport_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWInvestmentReport_Constants_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWInvestmentReport_Constants_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWInvestmentReport_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastWInvestmentReport_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWInvestmentReport_CostCenterTypeId",
                table: "CostForcastWInvestmentReport",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWInvestmentReport_OrganizationId",
                table: "CostForcastWInvestmentReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWInvestmentReport_SectionTypeId",
                table: "CostForcastWInvestmentReport",
                column: "SectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWInvestmentReport_UnitTypeId",
                table: "CostForcastWInvestmentReport",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastWInvestmentReport_YearId",
                table: "CostForcastWInvestmentReport",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastWInvestmentReport");
        }
    }
}
