using Microsoft.EntityFrameworkCore.Migrations;

namespace RealMadridWebApp.Migrations
{
    public partial class AddingPositionName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Country_BirthCountryCountryID",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_BirthCountryCountryID",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "BirthCountryCountryID",
                table: "Player");

            migrationBuilder.AddColumn<string>(
                name: "PositionName",
                table: "Position",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthCountryId",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Player_BirthCountryId",
                table: "Player",
                column: "BirthCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Country_BirthCountryId",
                table: "Player",
                column: "BirthCountryId",
                principalTable: "Country",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Country_BirthCountryId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_BirthCountryId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "PositionName",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "BirthCountryId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "BirthCountryCountryID",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_BirthCountryCountryID",
                table: "Player",
                column: "BirthCountryCountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Country_BirthCountryCountryID",
                table: "Player",
                column: "BirthCountryCountryID",
                principalTable: "Country",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
