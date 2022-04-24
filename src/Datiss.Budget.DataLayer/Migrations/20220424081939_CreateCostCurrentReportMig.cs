using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CreateCostCurrentReportMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentReports",
                columns: table => new
                {
                    CostCurrentReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SectionTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    FunctionalYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    FunctionalBasicYear = table.Column<long>(type: "bigint", nullable: false),
                    ApproveYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    ForcastY = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentReports", x => x.CostCurrentReportId);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_Constants_SectionTypeId",
                        column: x => x.SectionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_Constants_UnitDetailTypeId",
                        column: x => x.UnitDetailTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_Constants_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentReports_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_CostCenterTypeId",
                table: "CostCurrentReports",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_OrganizationId",
                table: "CostCurrentReports",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_SectionTypeId",
                table: "CostCurrentReports",
                column: "SectionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_UnitDetailTypeId",
                table: "CostCurrentReports",
                column: "UnitDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_UnitTypeId",
                table: "CostCurrentReports",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentReports_YearId",
                table: "CostCurrentReports",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentReports");
        }
    }
}
