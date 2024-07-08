using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionGenerator.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableOfContentsJson",
                table: "Document",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableOfContentsJson",
                table: "Document");
        }
    }
}
