using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SynergisticLogging.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreatedByFromDadJoke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DadJokes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DadJokes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
