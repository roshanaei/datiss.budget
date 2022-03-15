using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeTableNameMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacity");

            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacityCost");

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacityCurrent",
                columns: table => new
                {
                    UTACCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    AverageCapacityWIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_UserTypeAverageCapacityCurrent", x => x.UTACCId);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCurrent_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCurrent_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCurrent_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacityForcast",
                columns: table => new
                {
                    UTACFId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    AverageCapacityW = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWs = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_UserTypeAverageCapacityForcast", x => x.UTACFId);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityForcast_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityForcast_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityForcast_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCurrent_OrganizationId",
                table: "UserTypeAverageCapacityCurrent",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCurrent_UserTypeId",
                table: "UserTypeAverageCapacityCurrent",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCurrent_YearId",
                table: "UserTypeAverageCapacityCurrent",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityForcast_OrganizationId",
                table: "UserTypeAverageCapacityForcast",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityForcast_UserTypeId",
                table: "UserTypeAverageCapacityForcast",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityForcast_YearId",
                table: "UserTypeAverageCapacityForcast",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacityCurrent");

            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacityForcast");

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacity",
                columns: table => new
                {
                    UTACID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AverageCapacityW = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWs = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    YearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypeAverageCapacity", x => x.UTACID);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacity_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacityCost",
                columns: table => new
                {
                    UTACCID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AverageCapacityWIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AverageCapacityWsIncome = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedByBrowserName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModifiedByIp = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    YearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypeAverageCapacityCost", x => x.UTACCID);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCost_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCost_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTypeAverageCapacityCost_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_OrganizationId",
                table: "UserTypeAverageCapacity",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_UserTypeId",
                table: "UserTypeAverageCapacity",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_YearId",
                table: "UserTypeAverageCapacity",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCost_OrganizationId",
                table: "UserTypeAverageCapacityCost",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCost_UserTypeId",
                table: "UserTypeAverageCapacityCost",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacityCost_YearId",
                table: "UserTypeAverageCapacityCost",
                column: "YearId");
        }
    }
}
