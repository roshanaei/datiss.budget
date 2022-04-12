using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostForcastTransferWs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostForcastTransferWs",
                columns: table => new
                {
                    CFTWsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    TransferTypeId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreditTypeId = table.Column<int>(type: "int", nullable: false),
                    DigTypeId = table.Column<int>(type: "int", nullable: false),
                    MethodTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CostForcastTransferWs", x => x.CFTWsId);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_CreditTypeId",
                        column: x => x.CreditTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_DiameterPipeTypeId",
                        column: x => x.DiameterPipeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_DigTypeId",
                        column: x => x.DigTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_ExtensionTypeId",
                        column: x => x.ExtensionTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_MethodTypeId",
                        column: x => x.MethodTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_SuggestedBudgetTopicTypeId",
                        column: x => x.SuggestedBudgetTopicTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_TransferTypeId",
                        column: x => x.TransferTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Constants_TubeTypeId",
                        column: x => x.TubeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostForcastTransferWs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_CreditTypeId",
                table: "CostForcastTransferWs",
                column: "CreditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_DiameterPipeTypeId",
                table: "CostForcastTransferWs",
                column: "DiameterPipeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_DigTypeId",
                table: "CostForcastTransferWs",
                column: "DigTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_ExtensionTypeId",
                table: "CostForcastTransferWs",
                column: "ExtensionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_MethodTypeId",
                table: "CostForcastTransferWs",
                column: "MethodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_OrganizationId",
                table: "CostForcastTransferWs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_SuggestedBudgetTopicTypeId",
                table: "CostForcastTransferWs",
                column: "SuggestedBudgetTopicTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_TransferTypeId",
                table: "CostForcastTransferWs",
                column: "TransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_TubeTypeId",
                table: "CostForcastTransferWs",
                column: "TubeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostForcastTransferWs_YearId",
                table: "CostForcastTransferWs",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostForcastTransferWs");
        }
    }
}
