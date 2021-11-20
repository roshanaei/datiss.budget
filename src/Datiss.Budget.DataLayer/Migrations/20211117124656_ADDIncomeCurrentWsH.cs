using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ADDIncomeCurrentWsH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeCurrentWsH",
                columns: table => new
                {
                    IncomeCurrentWsHID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    TIncome = table.Column<int>(type: "int", nullable: false),
                    Note7Price = table.Column<int>(type: "int", nullable: false),
                    Note7Income = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IncomeCurrentWsH", x => x.IncomeCurrentWsHID);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_OrganizationId",
                table: "IncomeCurrentWsH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_UsageLayerId",
                table: "IncomeCurrentWsH",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_UserTypeId",
                table: "IncomeCurrentWsH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsH_YearId",
                table: "IncomeCurrentWsH",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeCurrentWsH");
        }
    }
}
