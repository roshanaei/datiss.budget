using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddConsumeForcastWs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumeForcastWs",
                columns: table => new
                {
                    ConsumeForcastWsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    UsageLayerId = table.Column<int>(type: "int", nullable: false),
                    CountUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AvgConsumeUser = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumeUserForcast = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_ConsumeForcastWs", x => x.ConsumeForcastWsId);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Constants_UsageLayerId",
                        column: x => x.UsageLayerId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumeForcastWs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_OrganizationId",
                table: "ConsumeForcastWs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_UsageLayerId",
                table: "ConsumeForcastWs",
                column: "UsageLayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_UserTypeId",
                table: "ConsumeForcastWs",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumeForcastWs_YearId",
                table: "ConsumeForcastWs",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumeForcastWs");
        }
    }
}
