using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakenlijstManager.Migrations
{
    /// <inheritdoc />
    public partial class statusTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Taken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Statussen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    VolgendeStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statussen", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Taken_StatusId",
                table: "Taken",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken",
                column: "StatusId",
                principalTable: "Statussen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken");

            migrationBuilder.DropTable(
                name: "Statussen");

            migrationBuilder.DropIndex(
                name: "IX_Taken_StatusId",
                table: "Taken");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Taken");
        }
    }
}
