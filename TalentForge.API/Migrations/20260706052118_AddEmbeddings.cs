using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentForge.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEmbeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmbeddingJson",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmbeddingJson",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddingJson",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EmbeddingJson",
                table: "Candidates");
        }
    }
}
