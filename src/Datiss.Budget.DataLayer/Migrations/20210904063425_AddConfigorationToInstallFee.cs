using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class AddConfigorationToInstallFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WInstllFee",
                table: "WasteInstallFees",
                newName: "WsInstllFee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WsInstllFee",
                table: "WasteInstallFees",
                newName: "WInstllFee");
        }
    }
}
