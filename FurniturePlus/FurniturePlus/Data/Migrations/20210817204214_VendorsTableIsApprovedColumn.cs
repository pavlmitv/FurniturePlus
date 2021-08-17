using Microsoft.EntityFrameworkCore.Migrations;

namespace FurniturePlus.Data.Migrations
{
    public partial class VendorsTableIsApprovedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Vendors");
        }
    }
}
