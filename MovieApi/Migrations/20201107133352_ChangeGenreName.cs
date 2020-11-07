using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApi.Migrations
{
    public partial class ChangeGenreName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genres",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genres");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
