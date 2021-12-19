using Microsoft.EntityFrameworkCore.Migrations;

namespace Datiss.Budget.DataLayer.Migrations
{
    public partial class ChangeWaterInstallFeeFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WInstllFee",
                table: "WaterInstallFees",
                newName: "WInstallFee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WInstallFee",
                table: "WaterInstallFees",
                newName: "WInstllFee");
        }
    }
}
