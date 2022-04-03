using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddRelationToConstantDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExtensionId",
                table: "CostCurrentContractual",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentContractual_ExtensionId",
                table: "CostCurrentContractual",
                column: "ExtensionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCurrentContractual_Constants_ExtensionId",
                table: "CostCurrentContractual",
                column: "ExtensionId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCurrentContractual_Constants_ExtensionId",
                table: "CostCurrentContractual");

            migrationBuilder.DropIndex(
                name: "IX_CostCurrentContractual_ExtensionId",
                table: "CostCurrentContractual");

            migrationBuilder.AlterColumn<bool>(
                name: "ExtensionId",
                table: "CostCurrentContractual",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
