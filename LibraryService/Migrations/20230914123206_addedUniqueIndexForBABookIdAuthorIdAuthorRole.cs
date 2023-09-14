using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryService.Migrations
{
    /// <inheritdoc />
    public partial class addedUniqueIndexForBABookIdAuthorIdAuthorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookAuthor_BookId",
                table: "BookAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_BookId_AuthorId_AuthorRole",
                table: "BookAuthor",
                columns: new[] { "BookId", "AuthorId", "AuthorRole" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookAuthor_BookId_AuthorId_AuthorRole",
                table: "BookAuthor");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_BookId",
                table: "BookAuthor",
                column: "BookId");
        }
    }
}
