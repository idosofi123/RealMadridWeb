using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class CompetitionInitMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "Match");
        }
    }
}
