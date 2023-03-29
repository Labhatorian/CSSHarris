using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CijferRegistratie.Migrations
{
    /// <inheritdoc />
    public partial class Pogingen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pogingen",
                columns: table => new
                {
                    PogingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jaar = table.Column<int>(type: "int", nullable: false),
                    Resultaat = table.Column<int>(type: "int", nullable: false),
                    VakNaam = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pogingen", x => x.PogingId);
                    table.ForeignKey(
                        name: "FK_Pogingen_Vakken_VakNaam",
                        column: x => x.VakNaam,
                        principalTable: "Vakken",
                        principalColumn: "Naam");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pogingen_VakNaam",
                table: "Pogingen",
                column: "VakNaam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pogingen");
        }
    }
}
