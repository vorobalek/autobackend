﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                name: "Budget",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    TimeZone = table.Column<TimeSpan>(type: "interval", nullable: true),
                    ActiveBudgetId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Budget_ActiveBudgetId",
                        column: x => x.ActiveBudgetId,
                        principalSchema: "generic",
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Participating",
                schema: "generic",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participating", x => new { x.UserId, x.BudgetId });
                    table.ForeignKey(
                        name: "FK_Participating_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "generic",
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participating_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "generic",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "generic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(20,4)", precision: 20, scale: 4, nullable: false),
                    DateTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comment = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    SecretKey = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "generic",
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "generic",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TransactionVersion",
                schema: "generic",
                columns: table => new
                {
                    __Generic__Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalTransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionDateTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    BudgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(20,4)", precision: 20, scale: 4, nullable: false),
                    DateTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comment = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionVersion", x => x.__Generic__Id);
                    table.ForeignKey(
                        name: "FK_TransactionVersion_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "generic",
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionVersion_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "generic",
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionVersion_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "generic",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budget_Name_OwnerId",
                schema: "generic",
                table: "Budget",
                columns: new[] { "Name", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Budget_OwnerId",
                schema: "generic",
                table: "Budget",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Participating_BudgetId",
                schema: "generic",
                table: "Participating",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BudgetId",
                schema: "generic",
                table: "Transaction",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserId",
                schema: "generic",
                table: "Transaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionVersion_BudgetId",
                schema: "generic",
                table: "TransactionVersion",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionVersion_TransactionId",
                schema: "generic",
                table: "TransactionVersion",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionVersion_UserId",
                schema: "generic",
                table: "TransactionVersion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ActiveBudgetId",
                schema: "generic",
                table: "User",
                column: "ActiveBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_User_OwnerId",
                schema: "generic",
                table: "Budget",
                column: "OwnerId",
                principalSchema: "generic",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_User_OwnerId",
                schema: "generic",
                table: "Budget");

            migrationBuilder.DropTable(
                name: "Participating",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "TransactionVersion",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "User",
                schema: "generic");

            migrationBuilder.DropTable(
                name: "Budget",
                schema: "generic");
        }
    }
}
