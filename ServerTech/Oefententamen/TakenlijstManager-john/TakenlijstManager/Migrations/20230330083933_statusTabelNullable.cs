using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TakenlijstManager.Migrations
{
    /// <inheritdoc />
    public partial class statusTabelNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Taken",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken",
                column: "StatusId",
                principalTable: "Statussen",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Taken",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Taken_Statussen_StatusId",
                table: "Taken",
                column: "StatusId",
                principalTable: "Statussen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
