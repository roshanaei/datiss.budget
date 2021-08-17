using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddWasteSalesSplit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesSplitWs_Y",
                columns: table => new
                {
                    SalesSplitWsYID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WsPipeDiameterId = table.Column<int>(type: "int", nullable: false),
                    NumberSales = table.Column<int>(type: "int", nullable: false),
                    UnitSales = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SalesSplitWs_Y", x => x.SalesSplitWsYID);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Constants_WsPipeDiameterId",
                        column: x => x.WsPipeDiameterId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitWs_Y_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_OrganizationId",
                table: "SalesSplitWs_Y",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_UserTypeId",
                table: "SalesSplitWs_Y",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_WsPipeDiameterId",
                table: "SalesSplitWs_Y",
                column: "WsPipeDiameterId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitWs_Y_YearId",
                table: "SalesSplitWs_Y",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesSplitWs_Y");
        }
    }
}
