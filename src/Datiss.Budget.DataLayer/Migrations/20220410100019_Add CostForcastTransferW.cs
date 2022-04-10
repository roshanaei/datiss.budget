using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastTransferW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastTransferW",
                columns: table => new
                {
                    CFCTWId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    TransferTypeId = table.Column<int>(type: "int", nullable: false),
                    CreaditTypeId = table.Column<int>(type: "int", nullable: false),
                    DigTypeId = table.Column<int>(type: "int", nullable: false),
                    TubeTypeId = table.Column<int>(type: "int", nullable: false),
                    DiameterPipeTypeId = table.Column<int>(type: "int", nullable: false),
                    Lenth = table.Column<int>(type: "int", nullable: false),
                    PipeCost = table.Column<long>(type: "bigint", nullable: false),
                    RunCost = table.Column<long>(type: "bigint", nullable: false),
                    TotalCost = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastTransferW", x => x.CFCTWId);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_CreaditTypeId",
                        column: x => x.CreaditTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_DiameterPipeTypeId",
                        column: x => x.DiameterPipeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_DigTypeId",
                        column: x => x.DigTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_ExtensionTypeId",
                        column: x => x.ExtensionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_SuggestedBudgetTopicTypeId",
                        column: x => x.SuggestedBudgetTopicTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_TransferTypeId",
                        column: x => x.TransferTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Constants_TubeTypeId",
                        column: x => x.TubeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferW_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_CreaditTypeId",
                table: "CostForcastTransferW",
                column: "CreaditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_DiameterPipeTypeId",
                table: "CostForcastTransferW",
                column: "DiameterPipeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_DigTypeId",
                table: "CostForcastTransferW",
                column: "DigTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_ExtensionTypeId",
                table: "CostForcastTransferW",
                column: "ExtensionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_OrganizationId",
                table: "CostForcastTransferW",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_SuggestedBudgetTopicTypeId",
                table: "CostForcastTransferW",
                column: "SuggestedBudgetTopicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_TransferTypeId",
                table: "CostForcastTransferW",
                column: "TransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_TubeTypeId",
                table: "CostForcastTransferW",
                column: "TubeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferW_YearId",
                table: "CostForcastTransferW",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastTransferW");
        }
    }
}
