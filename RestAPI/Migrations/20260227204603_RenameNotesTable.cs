using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "notes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notes",
                table: "notes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_notes",
                table: "notes");

            migrationBuilder.RenameTable(
                name: "notes",
                newName: "Notes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "Id");
        }
    }
}
