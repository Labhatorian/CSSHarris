using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakenlijstManager.Migrations
{
    /// <inheritdoc />
    public partial class taakPrioriteit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Prioriteit",
                table: "Taken",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prioriteit",
                table: "Taken");
        }
    }
}
