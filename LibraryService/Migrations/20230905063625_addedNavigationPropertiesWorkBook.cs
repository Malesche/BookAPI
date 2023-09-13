using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryService.Migrations
{
    /// <inheritdoc />
    public partial class addedNavigationPropertiesWorkBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_WorkId",
                table: "Books",
                column: "WorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Works_WorkId",
                table: "Books",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Works_WorkId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_WorkId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "Books");
        }
    }
}
