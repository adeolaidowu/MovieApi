using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApi.Migrations
{
    public partial class PopulateGenreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Genres (GenreId, Name) VALUES (1, 'Comedy')");
            migrationBuilder.Sql("INSERT INTO Genres (GenreId, Name) VALUES (2, 'Action')");
            migrationBuilder.Sql("INSERT INTO Genres (GenreId, Name) VALUES (3, 'Family')");
            migrationBuilder.Sql("INSERT INTO Genres (GenreId, Name) VALUES (4, 'Romance')");
            migrationBuilder.Sql("INSERT INTO Genres (GenreId, Name) VALUES (5, 'Fantasy')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
