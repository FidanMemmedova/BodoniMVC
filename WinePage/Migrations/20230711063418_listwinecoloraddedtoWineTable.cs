using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinePage.Migrations
{
    public partial class listwinecoloraddedtoWineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorWine");

            migrationBuilder.CreateTable(
                name: "WineColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WineId = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WineColor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WineColor_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WineColor_Wine_WineId",
                        column: x => x.WineId,
                        principalTable: "Wine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WineColor_ColorId",
                table: "WineColor",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_WineColor_WineId",
                table: "WineColor",
                column: "WineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WineColor");

            migrationBuilder.CreateTable(
                name: "ColorWine",
                columns: table => new
                {
                    ColorsId = table.Column<int>(type: "int", nullable: false),
                    WineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorWine", x => new { x.ColorsId, x.WineId });
                    table.ForeignKey(
                        name: "FK_ColorWine_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorWine_Wine_WineId",
                        column: x => x.WineId,
                        principalTable: "Wine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorWine_WineId",
                table: "ColorWine",
                column: "WineId");
        }
    }
}
