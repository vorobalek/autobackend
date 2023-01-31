using System;
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
                    Title = table.Column<string>(type: "text", nullable: true),
                    Artist = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
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
                name: "Book2Albums",
                schema: "generic",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book2Albums", x => new { x.BookId, x.AlbumId });
                    table.ForeignKey(
                        name: "FK_Book2Albums_Album_AlbumId",
                        column: x => x.AlbumId,
                        principalSchema: "generic",
                        principalTable: "Album",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book2Albums_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookShelve",
                schema: "generic",
                columns: table => new
                {
                    Book1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book2Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book3Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book4Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book5Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book6Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book7Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Book8Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookShelve", x => new { x.Book1Id, x.Book2Id, x.Book3Id, x.Book4Id, x.Book5Id, x.Book6Id, x.Book7Id, x.Book8Id });
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book1Id",
                        column: x => x.Book1Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book2Id",
                        column: x => x.Book2Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book3Id",
                        column: x => x.Book3Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book4Id",
                        column: x => x.Book4Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book5Id",
                        column: x => x.Book5Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book6Id",
                        column: x => x.Book6Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book7Id",
                        column: x => x.Book7Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookShelve_Book_Book8Id",
                        column: x => x.Book8Id,
                        principalSchema: "generic",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book2Albums_AlbumId",
                schema: "generic",
                table: "Book2Albums",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book2Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book2Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book3Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book3Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book4Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book4Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book5Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book5Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book6Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book6Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book7Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book7Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookShelve_Book8Id",
                schema: "generic",
                table: "BookShelve",
                column: "Book8Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book2Albums",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "BookShelve",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Note",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Album",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "generic");
        }
    }
}
