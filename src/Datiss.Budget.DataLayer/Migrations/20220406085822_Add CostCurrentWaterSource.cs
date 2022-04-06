using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCurrentWaterSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentWaterSource",
                columns: table => new
                {
                    CCWaterSourceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    WaterSourceTypeId = table.Column<int>(type: "int", nullable: false),
                    ActiveSource = table.Column<int>(type: "int", nullable: false),
                    BaseProduction = table.Column<long>(type: "bigint", nullable: false),
                    LastYearProduction = table.Column<long>(type: "bigint", nullable: false),
                    ForcastProduction = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentWaterSource", x => x.CCWaterSourceId);
                    table.ForeignKey(
                        name: "FK_CostCurrentWaterSource_Constants_WaterSourceTypeId",
                        column: x => x.WaterSourceTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentWaterSource_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentWaterSource_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentWaterSource_OrganizationId",
                table: "CostCurrentWaterSource",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentWaterSource_WaterSourceTypeId",
                table: "CostCurrentWaterSource",
                column: "WaterSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentWaterSource_YearId",
                table: "CostCurrentWaterSource",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentWaterSource");
        }
    }
}
