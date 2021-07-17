using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class OrganizationAddSewageStatusAndOrgType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVillage",
                table: "Organizations",
                newName: "SewageStatus");

            migrationBuilder.AddColumn<int>(
                name: "OrgType",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgType",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "SewageStatus",
                table: "Organizations",
                newName: "IsVillage");
        }
    }
}
