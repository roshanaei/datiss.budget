using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddBranchingRateIncrease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchingRateIncrease",
                columns: table => new
                {
                    BranchingRateIncreaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WaterRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WasteRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WastePersentIncrease = table.Column<int>(type: "int", nullable: false),
                    FixAmountBusiness = table.Column<int>(type: "int", nullable: false),
                    CapacityFixAmount = table.Column<int>(type: "int", nullable: false),
                    WaterInstallRateIncrease = table.Column<int>(type: "int", nullable: false),
                    WsInstalIncrease = table.Column<int>(type: "int", nullable: false),
                    WaterFixNote2 = table.Column<int>(type: "int", nullable: false),
                    WasteFixNote2 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BranchingRateIncrease", x => x.BranchingRateIncreaseId);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchingRateIncrease_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_OrganizationId",
                table: "BranchingRateIncrease",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_UserTypeId",
                table: "BranchingRateIncrease",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchingRateIncrease_YearId",
                table: "BranchingRateIncrease",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchingRateIncrease");
        }
    }
}
