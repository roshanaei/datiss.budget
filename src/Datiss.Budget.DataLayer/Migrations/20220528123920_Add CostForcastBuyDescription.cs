using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastBuyDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastBuyDescription",
                columns: table => new
                {
                    CFBDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    AssetTypeId = table.Column<int>(type: "int", nullable: false),
                    AssetDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastBuyDescription", x => x.CFBDId);
                    table.ForeignKey(
                        name: "FK_CostForcastBuyDescription_Constants_AssetDetailTypeId",
                        column: x => x.AssetDetailTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuyDescription_Constants_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuyDescription_Constants_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastBuyDescription_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuyDescription_AssetDetailTypeId",
                table: "CostForcastBuyDescription",
                column: "AssetDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuyDescription_AssetTypeId",
                table: "CostForcastBuyDescription",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuyDescription_MeasurementTypeId",
                table: "CostForcastBuyDescription",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastBuyDescription_YearId",
                table: "CostForcastBuyDescription",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastBuyDescription");
        }
    }
}
