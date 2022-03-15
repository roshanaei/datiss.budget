using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddIncomeCurrentCofficient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeCurrentCofficient",
                columns: table => new
                {
                    IncomeCurrentCofficientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_IncomeCurrentCofficient", x => x.IncomeCurrentCofficientId);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentCofficient_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentCofficient_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentCofficient_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeCurrentCofficient_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentCofficient_OrganizationId",
                table: "IncomeCurrentCofficient",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentCofficient_UsageLayerId",
                table: "IncomeCurrentCofficient",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentCofficient_UserTypeId",
                table: "IncomeCurrentCofficient",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeCurrentCofficient_YearId",
                table: "IncomeCurrentCofficient",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeCurrentCofficient");
        }
    }
}
