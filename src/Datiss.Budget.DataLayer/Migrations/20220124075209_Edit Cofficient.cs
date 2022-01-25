using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class EditCofficient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cofficients_Constants_CofficientId1",
                table: "Cofficients");

            migrationBuilder.RenameColumn(
                name: "CofficientId1",
                table: "Cofficients",
                newName: "CofficientTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cofficients_CofficientId1",
                table: "Cofficients",
                newName: "IX_Cofficients_CofficientTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cofficients_Constants_CofficientTypeId",
                table: "Cofficients",
                column: "CofficientTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cofficients_Constants_CofficientTypeId",
                table: "Cofficients");

            migrationBuilder.RenameColumn(
                name: "CofficientTypeId",
                table: "Cofficients",
                newName: "CofficientId1");

            migrationBuilder.RenameIndex(
                name: "IX_Cofficients_CofficientTypeId",
                table: "Cofficients",
                newName: "IX_Cofficients_CofficientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cofficients_Constants_CofficientId1",
                table: "Cofficients",
                column: "CofficientId1",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
