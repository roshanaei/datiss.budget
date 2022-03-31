using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCurrentFinancing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentFinancing",
                columns: table => new
                {
                    CCFinancingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    FinancialCostTypeId = table.Column<int>(type: "int", nullable: false),
                    BaseFee = table.Column<long>(type: "bigint", nullable: false),
                    LastYearFee = table.Column<long>(type: "bigint", nullable: false),
                    ForcastFee = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentFinancing", x => x.CCFinancingId);
                    table.ForeignKey(
                        name: "FK_CostCurrentFinancing_Constants_FinancialCostTypeId",
                        column: x => x.FinancialCostTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentFinancing_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentFinancing_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentFinancing_FinancialCostTypeId",
                table: "CostCurrentFinancing",
                column: "FinancialCostTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentFinancing_OrganizationId",
                table: "CostCurrentFinancing",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentFinancing_YearId",
                table: "CostCurrentFinancing",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentFinancing");
        }
    }
}
