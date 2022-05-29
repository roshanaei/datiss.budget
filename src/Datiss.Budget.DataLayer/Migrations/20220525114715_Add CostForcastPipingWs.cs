using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastPipingWs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastPipingWs",
                columns: table => new
                {
                    CFPWsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    TubeTypeId = table.Column<int>(type: "int", nullable: false),
                    DiameterPipeTypeId = table.Column<int>(type: "int", nullable: false),
                    DigTypeId = table.Column<int>(type: "int", nullable: false),
                    TubeBuyCost = table.Column<long>(type: "bigint", nullable: false),
                    NaghabCost = table.Column<long>(type: "bigint", nullable: false),
                    TeransheCost = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastPipingWs", x => x.CFPWsId);
                    table.ForeignKey(
                        name: "FK_CostForcastPipingWs_Constants_DiameterPipeTypeId",
                        column: x => x.DiameterPipeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastPipingWs_Constants_DigTypeId",
                        column: x => x.DigTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastPipingWs_Constants_TubeTypeId",
                        column: x => x.TubeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastPipingWs_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastPipingWs_DiameterPipeTypeId",
                table: "CostForcastPipingWs",
                column: "DiameterPipeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastPipingWs_DigTypeId",
                table: "CostForcastPipingWs",
                column: "DigTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastPipingWs_TubeTypeId",
                table: "CostForcastPipingWs",
                column: "TubeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastPipingWs_YearId",
                table: "CostForcastPipingWs",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastPipingWs");
        }
    }
}
