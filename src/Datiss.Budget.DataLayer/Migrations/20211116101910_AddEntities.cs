using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeCurrentWH",
                columns: table => new
                {
                    IncomeCurrentWHId = table.Column<int>(type: "int", nullable: false)
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
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    TIncome = table.Column<int>(type: "int", nullable: false),
                    Diff_ConsWsVolume = table.Column<int>(type: "int", nullable: false),
                    Note2Income = table.Column<int>(type: "int", nullable: false),
                    WasteVolume = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IncomeCurrentWH", x => x.IncomeCurrentWHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCurrentWNH",
                columns: table => new
                {
                    IncomeCurrentWNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberUser = table.Column<int>(type: "int", nullable: false),
                    UnitUser = table.Column<int>(type: "int", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumptionUser = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<int>(type: "int", nullable: false),
                    ExcessIncome = table.Column<int>(type: "int", nullable: false),
                    SeasonalIncome = table.Column<int>(type: "int", nullable: false),
                    Note3Price = table.Column<int>(type: "int", nullable: false),
                    Note3Income = table.Column<int>(type: "int", nullable: false),
                    SubscriptionIncome = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<int>(type: "int", nullable: false),
                    Diff_ConsWsVolume = table.Column<int>(type: "int", nullable: false),
                    Note2Income = table.Column<int>(type: "int", nullable: false),
                    WasteVolume = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IncomeCurrentWNH", x => x.IncomeCurrentWNHId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentWNH_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WWsFee_UsageLayerId",
                table: "WWsFee",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_OrganizationId",
                table: "IncomeCurrentWH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_UsageLayerId",
                table: "IncomeCurrentWH",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_UserTypeId",
                table: "IncomeCurrentWH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWH_YearId",
                table: "IncomeCurrentWH",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_OrganizationId",
                table: "IncomeCurrentWNH",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_UserTypeId",
                table: "IncomeCurrentWNH",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentWNH_YearId",
                table: "IncomeCurrentWNH",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_WWsFee_Constants_UsageLayerId",
                table: "WWsFee",
                column: "UsageLayerId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WWsFee_Constants_UsageLayerId",
                table: "WWsFee");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWH");

            migrationBuilder.DropTable(
                name: "IncomeCurrentWNH");

            migrationBuilder.DropIndex(
                name: "IX_WWsFee_UsageLayerId",
                table: "WWsFee");
        }
    }
}
