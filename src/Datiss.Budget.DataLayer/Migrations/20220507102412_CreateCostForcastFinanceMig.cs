using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CreateCostForcastFinanceMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastFinance",
                columns: table => new
                {
                    CostForcastFinanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    FinanceSubjectTypeId = table.Column<int>(type: "int", nullable: false),
                    RemainingAssets = table.Column<long>(type: "bigint", nullable: false),
                    AssetsCreated6_1 = table.Column<long>(type: "bigint", nullable: false),
                    AssetsCreated6_2 = table.Column<long>(type: "bigint", nullable: false),
                    ForcastAssets_D = table.Column<long>(type: "bigint", nullable: false),
                    TotalAssetsCreated_D = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastFinance", x => x.CostForcastFinanceId);
                    table.ForeignKey(
                        name: "FK_CostForcastFinance_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastFinance_Constants_FinanceSubjectTypeId",
                        column: x => x.FinanceSubjectTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastFinance_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastFinance_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastFinance_CostCenterTypeId",
                table: "CostForcastFinance",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastFinance_FinanceSubjectTypeId",
                table: "CostForcastFinance",
                column: "FinanceSubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastFinance_OrganizationId",
                table: "CostForcastFinance",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastFinance_YearId",
                table: "CostForcastFinance",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastFinance");
        }
    }
}
