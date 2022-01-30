using Microsoft.EntityFrameworkCore.Migrations;

namespace ElexirApp_RSWEB.Migrations
{
    public partial class UpdateRez : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hour",
                table: "Rezervacija",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Rezervacija");
        }
    }
}
