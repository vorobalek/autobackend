using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations.Postgres
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
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
