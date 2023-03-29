using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CijferRegistratie.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vakken",
                columns: table => new
                {
                    Naam = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EC = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vakken", x => x.Naam);
                });

            migrationBuilder.InsertData(
                table: "Vakken",
                columns: new[] { "Naam", "EC" },
                values: new object[,]
                {
                    { "C#", 4 },
                    { "Databases", 3 },
                    { "KBS", 9 },
                    { "Server", 4 },
                    { "UML", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vakken");
        }
    }
}
