using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApi.Migrations
{
    public partial class AddIpAddressToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AspNetUsers");
        }
    }
}
