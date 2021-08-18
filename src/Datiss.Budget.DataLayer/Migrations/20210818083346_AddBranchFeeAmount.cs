using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddBranchFeeAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterWasteBranchingAmount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UrbanAdjustmentFactor = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WasteRateInWater = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WaterBranchingPerHousing = table.Column<int>(type: "int", nullable: false),
                    TubingCost = table.Column<int>(type: "int", nullable: false),
                    WaterPartnershipAmountDomestic = table.Column<int>(type: "int", nullable: false),
                    WaterPartnershipAmountNDomestic = table.Column<int>(type: "int", nullable: false),
                    WastePartnershipAmountDomestic = table.Column<int>(type: "int", nullable: false),
                    WastePartnershipAmountNDomestic = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11H = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11NH = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11HWs = table.Column<int>(type: "int", nullable: false),
                    FixCostNote11NHWs = table.Column<int>(type: "int", nullable: false),
                    WsTubingCost = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WaterWasteBranchingAmount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterWasteBranchingAmount_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WaterWasteBranchingAmount_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterWasteBranchingAmount_OrganizationId",
                table: "WaterWasteBranchingAmount",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterWasteBranchingAmount_YearId",
                table: "WaterWasteBranchingAmount",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterWasteBranchingAmount");
        }
    }
}
