using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeFieldNameAddField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostForcastTransferW_Constants_CreaditTypeId",
                table: "CostForcastTransferW");

            migrationBuilder.RenameColumn(
                name: "CFCTWId",
                table: "CostForcastTransferW",
                newName: "CFTWId");

            migrationBuilder.RenameColumn(
                name: "CreaditTypeId",
                table: "CostForcastTransferW",
                newName: "CreditTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CostForcastTransferW_CreaditTypeId",
                table: "CostForcastTransferW",
                newName: "IX_CostForcastTransferW_CreditTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "CostForcastTransferW",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CostForcastTransferW_Constants_CreditTypeId",
                table: "CostForcastTransferW",
                column: "CreditTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostForcastTransferW_Constants_CreditTypeId",
                table: "CostForcastTransferW");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "CostForcastTransferW");

            migrationBuilder.RenameColumn(
                name: "CFTWId",
                table: "CostForcastTransferW",
                newName: "CFCTWId");

            migrationBuilder.RenameColumn(
                name: "CreditTypeId",
                table: "CostForcastTransferW",
                newName: "CreaditTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CostForcastTransferW_CreditTypeId",
                table: "CostForcastTransferW",
                newName: "IX_CostForcastTransferW_CreaditTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostForcastTransferW_Constants_CreaditTypeId",
                table: "CostForcastTransferW",
                column: "CreaditTypeId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
