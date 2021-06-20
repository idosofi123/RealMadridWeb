using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class CompetitionInitMatchLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Match_CompetitionId",
                table: "Match",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Competition_CompetitionId",
                table: "Match",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Competition_CompetitionId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_CompetitionId",
                table: "Match");
        }
    }
}
