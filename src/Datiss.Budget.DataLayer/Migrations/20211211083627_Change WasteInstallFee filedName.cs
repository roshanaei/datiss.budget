using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeWasteInstallFeefiledName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WsInstllFee",
                table: "WasteInstallFees",
                newName: "WsInstallFee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WsInstallFee",
                table: "WasteInstallFees",
                newName: "WsInstllFee");
        }
    }
}
