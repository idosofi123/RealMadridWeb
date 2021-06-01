using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class StadiumPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Stadium",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Stadium",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Stadium");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Stadium");
        }
    }
}
