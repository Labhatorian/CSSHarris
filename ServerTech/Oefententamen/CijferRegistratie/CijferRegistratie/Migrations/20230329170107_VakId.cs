using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CijferRegistratie.Migrations
{
    /// <inheritdoc />
    public partial class VakId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pogingen_Vakken_VakNaam",
                table: "Pogingen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vakken",
                table: "Vakken");

            migrationBuilder.DropIndex(
                name: "IX_Pogingen_VakNaam",
                table: "Pogingen");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "Naam",
                keyValue: "C#");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "Naam",
                keyValue: "Databases");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "Naam",
                keyValue: "KBS");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "Naam",
                keyValue: "Server");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "Naam",
                keyValue: "UML");

            migrationBuilder.DropColumn(
                name: "VakNaam",
                table: "Pogingen");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Vakken",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "VakId",
                table: "Vakken",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "VakId",
                table: "Pogingen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vakken",
                table: "Vakken",
                column: "VakId");

            migrationBuilder.InsertData(
                table: "Vakken",
                columns: new[] { "VakId", "EC", "Naam" },
                values: new object[,]
                {
                    { 1, 4, "Server" },
                    { 2, 4, "C#" },
                    { 3, 3, "Databases" },
                    { 4, 3, "UML" },
                    { 5, 9, "KBS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pogingen_VakId",
                table: "Pogingen",
                column: "VakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pogingen_Vakken_VakId",
                table: "Pogingen",
                column: "VakId",
                principalTable: "Vakken",
                principalColumn: "VakId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pogingen_Vakken_VakId",
                table: "Pogingen");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vakken",
                table: "Vakken");

            migrationBuilder.DropIndex(
                name: "IX_Pogingen_VakId",
                table: "Pogingen");

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "VakId",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "VakId",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "VakId",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "VakId",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vakken",
                keyColumn: "VakId",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "VakId",
                table: "Vakken");

            migrationBuilder.DropColumn(
                name: "VakId",
                table: "Pogingen");

            migrationBuilder.AlterColumn<string>(
                name: "Naam",
                table: "Vakken",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "VakNaam",
                table: "Pogingen",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vakken",
                table: "Vakken",
                column: "Naam");

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

            migrationBuilder.CreateIndex(
                name: "IX_Pogingen_VakNaam",
                table: "Pogingen",
                column: "VakNaam");

            migrationBuilder.AddForeignKey(
                name: "FK_Pogingen_Vakken_VakNaam",
                table: "Pogingen",
                column: "VakNaam",
                principalTable: "Vakken",
                principalColumn: "Naam");
        }
    }
}
