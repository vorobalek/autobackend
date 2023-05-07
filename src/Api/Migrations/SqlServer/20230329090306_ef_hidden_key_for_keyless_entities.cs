using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class ef_hidden_key_for_keyless_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "__Generic__Id",
                schema: "generic",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                schema: "generic",
                table: "Note",
                column: "__Generic__Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                schema: "generic",
                table: "Note");

            migrationBuilder.DropColumn(
                name: "__Generic__Id",
                schema: "generic",
                table: "Note");
        }
    }
}
