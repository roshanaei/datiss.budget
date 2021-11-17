using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class SalesSplitTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesSplitTotal",
                columns: table => new
                {
                    SalesSplitTotalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    WNumber = table.Column<int>(type: "int", nullable: false),
                    WUnit = table.Column<int>(type: "int", nullable: false),
                    WsNumber = table.Column<int>(type: "int", nullable: false),
                    WsUnit = table.Column<int>(type: "int", nullable: false),
                    WNumber_2 = table.Column<int>(type: "int", nullable: false),
                    WUnit_2 = table.Column<int>(type: "int", nullable: false),
                    WsNumber_2 = table.Column<int>(type: "int", nullable: false),
                    WsUnit_2 = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SalesSplitTotal", x => x.SalesSplitTotalId);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_Constants_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesSplitTotal_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_OrganizationId",
                table: "SalesSplitTotal",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_UserTypeId",
                table: "SalesSplitTotal",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSplitTotal_YearId",
                table: "SalesSplitTotal",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesSplitTotal");
        }
    }
}
