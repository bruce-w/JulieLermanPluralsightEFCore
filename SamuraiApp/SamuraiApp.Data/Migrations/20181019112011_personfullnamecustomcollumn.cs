using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class personfullnamecustomcollumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "Samurais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "Samurais",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "Samurais");
        }
    }
}
