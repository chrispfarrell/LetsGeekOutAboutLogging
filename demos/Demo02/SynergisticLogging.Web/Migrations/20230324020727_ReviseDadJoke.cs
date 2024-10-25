using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SynergisticLogging.Web.Migrations
{
    /// <inheritdoc />
    public partial class ReviseDadJoke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "DadJokes",
                newName: "Setup");

            migrationBuilder.AddColumn<string>(
                name: "Punchline",
                table: "DadJokes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Punchline",
                table: "DadJokes");

            migrationBuilder.RenameColumn(
                name: "Setup",
                table: "DadJokes",
                newName: "Text");
        }
    }
}
