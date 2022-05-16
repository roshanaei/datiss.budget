using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddParentIdFieldToPMDepMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CostCurrentPMDeps",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCurrentPMDeps_ParentId",
                table: "CostCurrentPMDeps",
                column: "ParentId",
                unique: true,
                filter: "[ParentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCurrentPMDeps_CostCurrentPMDeps_ParentId",
                table: "CostCurrentPMDeps",
                column: "ParentId",
                principalTable: "CostCurrentPMDeps",
                principalColumn: "CostCurrentPMDepId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCurrentPMDeps_CostCurrentPMDeps_ParentId",
                table: "CostCurrentPMDeps");

            migrationBuilder.DropIndex(
                name: "IX_CostCurrentPMDeps_ParentId",
                table: "CostCurrentPMDeps");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CostCurrentPMDeps");
        }
    }
}
