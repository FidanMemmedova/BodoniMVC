using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinePage.Migrations
{
    public partial class addedDescriptiontoWineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Wine",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Wine");
        }
    }
}
