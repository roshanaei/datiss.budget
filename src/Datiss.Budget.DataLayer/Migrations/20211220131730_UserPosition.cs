using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class UserPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_PositionId",
                table: "AppUsers",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Constants_PositionId",
                table: "AppUsers",
                column: "PositionId",
                principalTable: "Constants",
                principalColumn: "ConstantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Constants_PositionId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_PositionId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "AppUsers");
        }
    }
}
