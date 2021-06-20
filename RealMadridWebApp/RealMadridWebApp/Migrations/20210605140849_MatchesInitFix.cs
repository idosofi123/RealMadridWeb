using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class MatchesInitFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAway",
                table: "Match",
                newName: "isAwayMatch");

            migrationBuilder.RenameColumn(
                name: "GoalsHome",
                table: "Match",
                newName: "HomeGoals");

            migrationBuilder.RenameColumn(
                name: "GoalsAway",
                table: "Match",
                newName: "AwayGoals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAwayMatch",
                table: "Match",
                newName: "isAway");

            migrationBuilder.RenameColumn(
                name: "HomeGoals",
                table: "Match",
                newName: "GoalsHome");

            migrationBuilder.RenameColumn(
                name: "AwayGoals",
                table: "Match",
                newName: "GoalsAway");
        }
    }
}
