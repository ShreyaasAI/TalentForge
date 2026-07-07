using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentForge.API.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "Candidates");
        }
    }
}
