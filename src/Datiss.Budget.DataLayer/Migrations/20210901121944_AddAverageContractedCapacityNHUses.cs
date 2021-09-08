using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddAverageContractedCapacityNHUses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_UserTypeAverageCapacity_Y_ParentId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropIndex(
                name: "IX_UserTypeAverageCapacity_Y_ParentId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.RenameColumn(
                name: "TableName",
                table: "UserTypeAverageCapacity_Y",
                newName: "YearId");

            migrationBuilder.RenameColumn(
                name: "SectionName",
                table: "UserTypeAverageCapacity_Y",
                newName: "UserTypeId");

            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "UserTypeAverageCapacity_Y",
                newName: "OrganizationId");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacity",
                table: "UserTypeAverageCapacity_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacityIncome",
                table: "UserTypeAverageCapacity_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacityWs",
                table: "UserTypeAverageCapacity_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageCapacityWsIncome",
                table: "UserTypeAverageCapacity_Y",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TablesFiledTitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TableName = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TablesFiledTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TablesFiledTitle_TablesFiledTitle_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TablesFiledTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_Y_OrganizationId",
                table: "UserTypeAverageCapacity_Y",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_Y_YearId",
                table: "UserTypeAverageCapacity_Y",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_TablesFiledTitle_ParentId",
                table: "TablesFiledTitle",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_Constants_OrganizationId",
                table: "UserTypeAverageCapacity_Y",
                column: "OrganizationId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_FinanceYears_YearId",
                table: "UserTypeAverageCapacity_Y",
                column: "YearId",
                principalTable: "FinanceYears",
                principalColumn: "FinanceYearId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_Organizations_OrganizationId",
                table: "UserTypeAverageCapacity_Y",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_Constants_OrganizationId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_FinanceYears_YearId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_Organizations_OrganizationId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropTable(
                name: "TablesFiledTitle");

            migrationBuilder.DropIndex(
                name: "IX_UserTypeAverageCapacity_Y_OrganizationId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropIndex(
                name: "IX_UserTypeAverageCapacity_Y_YearId",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "AverageCapacity",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "AverageCapacityIncome",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "AverageCapacityWs",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.DropColumn(
                name: "AverageCapacityWsIncome",
                table: "UserTypeAverageCapacity_Y");

            migrationBuilder.RenameColumn(
                name: "YearId",
                table: "UserTypeAverageCapacity_Y",
                newName: "TableName");

            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "UserTypeAverageCapacity_Y",
                newName: "SectionName");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "UserTypeAverageCapacity_Y",
                newName: "DisplayOrder");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "UserTypeAverageCapacity_Y",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserTypeAverageCapacity_Y",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UserTypeAverageCapacity_Y",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypeAverageCapacity_Y_ParentId",
                table: "UserTypeAverageCapacity_Y",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTypeAverageCapacity_Y_UserTypeAverageCapacity_Y_ParentId",
                table: "UserTypeAverageCapacity_Y",
                column: "ParentId",
                principalTable: "UserTypeAverageCapacity_Y",
                principalColumn: "UTACY_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
