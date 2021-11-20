using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ADDIncomeCurrentWsNH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeCurrentWsNH",
                columns: table => new
                {
                    IncomeCurrentWsNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    ExcessIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IncomeCurrentWsNH", x => x.IncomeCurrentWsNHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWsNH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_OrganizationId",
                table: "IncomeCurrentWsNH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_UserTypeId",
                table: "IncomeCurrentWsNH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWsNH_YearId",
                table: "IncomeCurrentWsNH",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeCurrentWsNH");
        }
    }
}
