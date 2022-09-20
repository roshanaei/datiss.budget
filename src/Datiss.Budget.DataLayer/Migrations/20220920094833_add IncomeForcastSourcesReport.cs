using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class addIncomeForcastSourcesReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeForcastSourcesReport",
                columns: table => new
                {
                    IFSRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    SourceDescriptionId = table.Column<int>(type: "int", nullable: false),
                    FunctionalBasicYear = table.Column<long>(type: "bigint", nullable: false),
                    FunctionalLastYear = table.Column<long>(type: "bigint", nullable: false),
                    ApproveYear_1 = table.Column<long>(type: "bigint", nullable: false),
                    PercentBudget = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_IncomeForcastSourcesReport", x => x.IFSRId);
                    table.ForeignKey(
                        name: "FK_IncomeForcastSourcesReport_Constants_SourceDescriptionId",
                        column: x => x.SourceDescriptionId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastSourcesReport_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeForcastSourcesReport_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastSourcesReport_OrganizationId",
                table: "IncomeForcastSourcesReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastSourcesReport_SourceDescriptionId",
                table: "IncomeForcastSourcesReport",
                column: "SourceDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeForcastSourcesReport_YearId",
                table: "IncomeForcastSourcesReport",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeForcastSourcesReport");
        }
    }
}
