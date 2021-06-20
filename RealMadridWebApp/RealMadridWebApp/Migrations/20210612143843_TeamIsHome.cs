using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class TeamIsHome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Team",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Team");
        }
    }
}
