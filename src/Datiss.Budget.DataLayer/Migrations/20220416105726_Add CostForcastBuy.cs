using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastBuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastBuy",
                columns: table => new
                {
                    CFBuyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    BuyDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    BuyDepartmentId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    AssetTypeId = table.Column<int>(type: "int", nullable: false),
                    AssetDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    CreditTypeId = table.Column<int>(type: "int", nullable: false),
                    ProposedCost = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastBuy", x => x.CFBuyId);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_AssetDetailTypeId",
                        column: x => x.AssetDetailTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_BuyDepartmentId",
                        column: x => x.BuyDepartmentId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_CreditTypeId",
                        column: x => x.CreditTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Constants_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Organizations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuy_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_AssetDetailTypeId",
                table: "CostForcastBuy",
                column: "AssetDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_AssetTypeId",
                table: "CostForcastBuy",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_BuyDepartmentId",
                table: "CostForcastBuy",
                column: "BuyDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_CostCenterTypeId",
                table: "CostForcastBuy",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_CreditTypeId",
                table: "CostForcastBuy",
                column: "CreditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_LocationId",
                table: "CostForcastBuy",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_MeasurementTypeId",
                table: "CostForcastBuy",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_OrganizationId",
                table: "CostForcastBuy",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuy_YearId",
                table: "CostForcastBuy",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastBuy");
        }
    }
}
