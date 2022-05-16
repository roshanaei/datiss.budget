using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddFieldToPersonelMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CostCurrentPersonel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPersonel_ParentId",
                table: "CostCurrentPersonel",
                column: "ParentId",
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCurrentPersonel_CostCurrentPersonel_ParentId",
                table: "CostCurrentPersonel",
                column: "ParentId",
                principalTable: "CostCurrentPersonel",
                principalColumn: "PersonelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCurrentPersonel_CostCurrentPersonel_ParentId",
                table: "CostCurrentPersonel");

            migrationBuilder.DropIndex(
                name: "IX_CostCurrentPersonel_ParentId",
                table: "CostCurrentPersonel");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CostCurrentPersonel");
        }
    }
}
