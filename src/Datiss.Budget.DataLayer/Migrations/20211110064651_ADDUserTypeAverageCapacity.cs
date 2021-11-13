using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ADDUserTypeAverageCapacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTypeAverageCapacity",
                columns: table => new
                {
                    UTACID = table.Column<int>(type: "int", nullable: false)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTypeAverageCapacity");
        }
    }
}
