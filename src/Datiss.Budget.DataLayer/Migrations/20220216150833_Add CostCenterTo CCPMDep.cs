using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddCostCenterToCCPMDep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "CostCurrentPMDeps");

            migrationBuilder.AlterColumn<long>(
                name: "TotalCost",
                table: "IncomeCurrentOperationals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "PriceNH",
                table: "IncomeCurrentOperationals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "PriceH",
                table: "IncomeCurrentOperationals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CostNH",
                table: "IncomeCurrentOperationals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CostH",
                table: "IncomeCurrentOperationals",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CostCenterTypeId",
                table: "CostCurrentPMDeps",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPMDeps_CostCenterTypeId",
                table: "CostCurrentPMDeps",
                column: "CostCenterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCurrentPMDeps_Constants_CostCenterTypeId",
                table: "CostCurrentPMDeps",
                column: "CostCenterTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCurrentPMDeps_Constants_CostCenterTypeId",
                table: "CostCurrentPMDeps");

            migrationBuilder.DropIndex(
                name: "IX_CostCurrentPMDeps_CostCenterTypeId",
                table: "CostCurrentPMDeps");

            migrationBuilder.DropColumn(
                name: "CostCenterTypeId",
                table: "CostCurrentPMDeps");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCost",
                table: "IncomeCurrentOperationals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "PriceNH",
                table: "IncomeCurrentOperationals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "PriceH",
                table: "IncomeCurrentOperationals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CostNH",
                table: "IncomeCurrentOperationals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CostH",
                table: "IncomeCurrentOperationals",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CostCenter",
                table: "CostCurrentPMDeps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
