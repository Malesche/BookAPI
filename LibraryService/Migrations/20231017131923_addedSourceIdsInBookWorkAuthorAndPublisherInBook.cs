using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryService.Migrations
{
    /// <inheritdoc />
    public partial class addedSourceIdsInBookWorkAuthorAndPublisherInBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceIds",
                table: "Works",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIds",
                table: "Books",
                type: "NVARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIds",
                table: "Authors",
                type: "NVARCHAR(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceIds",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SourceIds",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SourceIds",
                table: "Authors");
        }
    }
}
