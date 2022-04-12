using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCurrentPersonel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCurrentPersonel",
                columns: table => new
                {
                    PersonelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    RecordType = table.Column<int>(type: "int", nullable: false),
                    PersonelCode = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<bool>(type: "bit", nullable: false),
                    GradeTypeId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentTypeId = table.Column<int>(type: "int", nullable: false),
                    CostCenterTypeId = table.Column<int>(type: "int", nullable: false),
                    JobStatusTypeId = table.Column<int>(type: "int", nullable: false),
                    JobStatusDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    ExperienceYear = table.Column<int>(type: "int", nullable: false),
                    ExperienceMonth = table.Column<int>(type: "int", nullable: false),
                    FixSalary = table.Column<long>(type: "bigint", nullable: false),
                    EmployRight = table.Column<long>(type: "bigint", nullable: false),
                    RegionRight = table.Column<long>(type: "bigint", nullable: false),
                    OverTimeValue = table.Column<int>(type: "int", nullable: false),
                    OverTimeCost = table.Column<long>(type: "bigint", nullable: false),
                    HolidayValue = table.Column<int>(type: "int", nullable: false),
                    HolidayCost = table.Column<long>(type: "bigint", nullable: false),
                    ShiftPercent = table.Column<long>(type: "bigint", nullable: false),
                    ShiftPCost = table.Column<long>(type: "bigint", nullable: false),
                    MissionCount = table.Column<long>(type: "bigint", nullable: false),
                    MissionDayCost = table.Column<long>(type: "bigint", nullable: false),
                    HardWorkingRt = table.Column<long>(type: "bigint", nullable: false),
                    TrafficRt = table.Column<long>(type: "bigint", nullable: false),
                    HouseRt = table.Column<long>(type: "bigint", nullable: false),
                    ChildRt = table.Column<long>(type: "bigint", nullable: false),
                    StuffRt = table.Column<long>(type: "bigint", nullable: false),
                    Education = table.Column<long>(type: "bigint", nullable: false),
                    InsuranceMaster = table.Column<long>(type: "bigint", nullable: false),
                    InsuranceAging = table.Column<long>(type: "bigint", nullable: false),
                    HolidayYearly = table.Column<long>(type: "bigint", nullable: false),
                    MilitaryServiceCost = table.Column<long>(type: "bigint", nullable: false),
                    UnUseHolidayCount = table.Column<long>(type: "bigint", nullable: false),
                    WelfareCost = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CostCurrentPersonel", x => x.PersonelId);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_CostCenterTypeId",
                        column: x => x.CostCenterTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_GradeTypeId",
                        column: x => x.GradeTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_JobDepartmentTypeId",
                        column: x => x.JobDepartmentTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_JobStatusDetailTypeId",
                        column: x => x.JobStatusDetailTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Constants_JobStatusTypeId",
                        column: x => x.JobStatusTypeId,
                        principalTable: "Constants",
                        principalColumn: "ConstantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_FinanceYears_YearId",
                        column: x => x.YearId,
                        principalTable: "FinanceYears",
                        principalColumn: "FinanceYearId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCurrentPersonel_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_ContractTypeId",
                table: "CostCurrentPersonel",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_CostCenterTypeId",
                table: "CostCurrentPersonel",
                column: "CostCenterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_GradeTypeId",
                table: "CostCurrentPersonel",
                column: "GradeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_JobDepartmentTypeId",
                table: "CostCurrentPersonel",
                column: "JobDepartmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_JobStatusDetailTypeId",
                table: "CostCurrentPersonel",
                column: "JobStatusDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_JobStatusTypeId",
                table: "CostCurrentPersonel",
                column: "JobStatusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_OrganizationId",
                table: "CostCurrentPersonel",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_YearId",
                table: "CostCurrentPersonel",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCurrentPersonel");
        }
    }
}
