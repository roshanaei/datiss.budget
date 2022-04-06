using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCurrentConstructionW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentConstructionW",
                columns: table => new
                {
                    CCCWId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WaterInvestorsTypeId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    ExploitationAreaTypeId = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false),
                    CostDone = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TotalCost = table.Column<long>(type: "bigint", nullable: false),
                    CreditTypeId = table.Column<int>(type: "int", nullable: false),
                    ExtensionTypeId = table.Column<int>(type: "int", nullable: false),
                    SuggestedBudgetTopicTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentConstructionW", x => x.CCCWId);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_CreditTypeId",
                        column: x => x.CreditTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_ExploitationAreaTypeId",
                        column: x => x.ExploitationAreaTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_ExtensionTypeId",
                        column: x => x.ExtensionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_MeasurementTypeId",
                        column: x => x.MeasurementTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_SuggestedBudgetTopicTypeId",
                        column: x => x.SuggestedBudgetTopicTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Constants_WaterInvestorsTypeId",
                        column: x => x.WaterInvestorsTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentConstructionW_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_CostCenterTypeId",
                table: "CostCurrentConstructionW",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_CreditTypeId",
                table: "CostCurrentConstructionW",
                column: "CreditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_ExploitationAreaTypeId",
                table: "CostCurrentConstructionW",
                column: "ExploitationAreaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_ExtensionTypeId",
                table: "CostCurrentConstructionW",
                column: "ExtensionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_MeasurementTypeId",
                table: "CostCurrentConstructionW",
                column: "MeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_OrganizationId",
                table: "CostCurrentConstructionW",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_SuggestedBudgetTopicTypeId",
                table: "CostCurrentConstructionW",
                column: "SuggestedBudgetTopicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_WaterInvestorsTypeId",
                table: "CostCurrentConstructionW",
                column: "WaterInvestorsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentConstructionW_YearId",
                table: "CostCurrentConstructionW",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentConstructionW");
        }
    }
}
