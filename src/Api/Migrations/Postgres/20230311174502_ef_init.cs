using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ef_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "generic");

            migrationBuilder.CreateTable(
                name: "Album",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Artist = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                schema: "generic",
                columns: table => new
                {
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Song",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AlbumContent",
                schema: "generic",
                columns: table => new
                {
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false),
                    SongId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumContent", x => new { x.AlbumId, x.SongId });
                    table.ForeignKey(
                        name: "FK_AlbumContent_Album_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "generic",
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumContent_Song_SongId",
                        column: x => x.SongId,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumSet",
                schema: "generic",
                columns: table => new
                {
                    Album1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album2Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album3Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album4Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album5Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album6Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album7Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Album8Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumSet", x => new { x.Album1Id, x.Album2Id, x.Album3Id, x.Album4Id, x.Album5Id, x.Album6Id, x.Album7Id, x.Album8Id });
                    table.ForeignKey(
                        name: "FK_AlbumSet_Album_Album1Id",
                        column: x => x.Album1Id,
                        principalSchema: "generic",
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Album_Album2Id",
                        column: x => x.Album2Id,
                        principalSchema: "generic",
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album3Id",
                        column: x => x.Album3Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album4Id",
                        column: x => x.Album4Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album5Id",
                        column: x => x.Album5Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album6Id",
                        column: x => x.Album6Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album7Id",
                        column: x => x.Album7Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AlbumSet_Song_Album8Id",
                        column: x => x.Album8Id,
                        principalSchema: "generic",
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumContent_SongId",
                schema: "generic",
                table: "AlbumContent",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album2Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album3Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album3Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album4Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album4Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album5Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album5Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album6Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album6Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album7Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album7Id");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSet_Album8Id",
                schema: "generic",
                table: "AlbumSet",
                column: "Album8Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumContent",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "AlbumSet",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Album",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Song",
                schema: "generic");
        }
    }
}
