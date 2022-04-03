using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCurrentSharingSetad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentSharingSetad",
                columns: table => new
                {
                    CostCurrentSharingSetadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    WUnit = table.Column<int>(type: "int", nullable: false),
                    IncomeCurrentW = table.Column<long>(type: "bigint", nullable: false),
                    IncomeCurrentWSharingCoff = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WsUnit = table.Column<int>(type: "int", nullable: false),
                    IncomeCurrentWs = table.Column<long>(type: "bigint", nullable: false),
                    IncomeCurrentWsSharingCoff = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IncomeForcast = table.Column<long>(type: "bigint", nullable: false),
                    SPSHahrdari = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    IncomeForcastsharing = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentSharingSetad", x => x.CostCurrentSharingSetadId);
                    table.ForeignKey(
                        name: "FK_CostCurrentSharingSetad_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentSharingSetad_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentSharingSetad_OrganizationId",
                table: "CostCurrentSharingSetad",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentSharingSetad_YearId",
                table: "CostCurrentSharingSetad",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentSharingSetad");
        }
    }
}
