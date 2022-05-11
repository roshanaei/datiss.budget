using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class CreateTableMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentPrescriptionBaseInfo",
                columns: table => new
                {
                    CostCurrentPrescriptionBaseInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    FixSalary = table.Column<long>(type: "bigint", nullable: false),
                    HouseRt = table.Column<long>(type: "bigint", nullable: false),
                    EmployRight = table.Column<long>(type: "bigint", nullable: false),
                    RegionRight = table.Column<long>(type: "bigint", nullable: false),
                    Copun = table.Column<int>(type: "int", nullable: false),
                    ChildRt = table.Column<long>(type: "bigint", nullable: false),
                    StuffRt = table.Column<long>(type: "bigint", nullable: false),
                    HardWorkingRt = table.Column<long>(type: "bigint", nullable: false),
                    Healths = table.Column<long>(type: "bigint", nullable: false),
                    NewFixSalary = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentPrescriptionBaseInfo", x => x.CostCurrentPrescriptionBaseInfoId);
                    table.ForeignKey(
                        name: "FK_CostCurrentPrescriptionBaseInfo_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPrescriptionBaseInfo_YearId",
                table: "CostCurrentPrescriptionBaseInfo",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentPrescriptionBaseInfo");
        }
    }
}
