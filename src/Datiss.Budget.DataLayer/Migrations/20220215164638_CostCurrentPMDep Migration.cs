using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CostCurrentPMDepMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentPMDeps",
                columns: table => new
                {
                    CostCurrentPMDepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CCPMDepTypeId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RecordType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CostCenter = table.Column<long>(type: "bigint", nullable: false),
                    FinancePMCost = table.Column<long>(type: "bigint", nullable: false),
                    RFinancePMCost_D = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    FinanceDepCost = table.Column<long>(type: "bigint", nullable: false),
                    RFinanceDepCost_D = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentPMDeps", x => x.CostCurrentPMDepId);
                    table.ForeignKey(
                        name: "FK_CostCurrentPMDeps_Constants_CCPMDepTypeId",
                        column: x => x.CCPMDepTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPMDeps_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPMDeps_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPMDeps_CCPMDepTypeId",
                table: "CostCurrentPMDeps",
                column: "CCPMDepTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPMDeps_OrganizationId",
                table: "CostCurrentPMDeps",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPMDeps_YearId",
                table: "CostCurrentPMDeps",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentPMDeps");
        }
    }
}
