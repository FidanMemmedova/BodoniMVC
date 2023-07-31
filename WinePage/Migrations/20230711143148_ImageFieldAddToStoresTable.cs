using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinePage.Migrations
{
    public partial class ImageFieldAddToStoresTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Stores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Stores");
        }
    }
}
