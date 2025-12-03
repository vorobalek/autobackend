#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Migrations.SqlServer;

/// <inheritdoc />
public partial class ef_init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            "generic");

        migrationBuilder.CreateTable(
            "Budget",
            schema: "generic",
            columns: table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                Name = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: false),
                OwnerId = table.Column<long>("bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Budget", x => x.Id);
            });

        migrationBuilder.CreateTable(
            "User",
            schema: "generic",
            columns: table => new
            {
                Id = table.Column<long>("bigint", nullable: false),
                FirstName = table.Column<string>("nvarchar(max)", nullable: true),
                LastName = table.Column<string>("nvarchar(max)", nullable: true),
                TimeZone = table.Column<TimeSpan>("time", nullable: true),
                ActiveBudgetId = table.Column<Guid>("uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_User", x => x.Id);
                table.ForeignKey(
                    "FK_User_Budget_ActiveBudgetId",
                    x => x.ActiveBudgetId,
                    principalSchema: "generic",
                    principalTable: "Budget",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            "Participating",
            schema: "generic",
            columns: table => new
            {
                UserId = table.Column<long>("bigint", nullable: false),
                BudgetId = table.Column<Guid>("uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Participating", x => new
                {
                    x.UserId,
                    x.BudgetId
                });
                table.ForeignKey(
                    "FK_Participating_Budget_BudgetId",
                    x => x.BudgetId,
                    principalSchema: "generic",
                    principalTable: "Budget",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_Participating_User_UserId",
                    x => x.UserId,
                    principalSchema: "generic",
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "Transaction",
            schema: "generic",
            columns: table => new
            {
                Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                UserId = table.Column<long>("bigint", nullable: true),
                BudgetId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Amount = table.Column<decimal>("decimal(20,4)", precision: 20, scale: 4, nullable: false),
                DateTimeUtc = table.Column<DateTime>("datetime2", nullable: false),
                Comment = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: false),
                SecretKey = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transaction", x => x.Id);
                table.ForeignKey(
                    "FK_Transaction_Budget_BudgetId",
                    x => x.BudgetId,
                    principalSchema: "generic",
                    principalTable: "Budget",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "FK_Transaction_User_UserId",
                    x => x.UserId,
                    principalSchema: "generic",
                    principalTable: "User",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            "TransactionVersion",
            schema: "generic",
            columns: table => new
            {
                __Generic__Id = table.Column<int>("int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TransactionId = table.Column<Guid>("uniqueidentifier", nullable: false),
                OriginalTransactionId = table.Column<Guid>("uniqueidentifier", nullable: false),
                VersionDateTimeUtc = table.Column<DateTime>("datetime2", nullable: false),
                UserId = table.Column<long>("bigint", nullable: true),
                BudgetId = table.Column<Guid>("uniqueidentifier", nullable: false),
                Amount = table.Column<decimal>("decimal(20,4)", precision: 20, scale: 4, nullable: false),
                DateTimeUtc = table.Column<DateTime>("datetime2", nullable: false),
                Comment = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TransactionVersion", x => x.__Generic__Id);
                table.ForeignKey(
                    "FK_TransactionVersion_Transaction_TransactionId",
                    x => x.TransactionId,
                    principalSchema: "generic",
                    principalTable: "Transaction",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "IX_Budget_Name_OwnerId",
            schema: "generic",
            table: "Budget",
            columns: new[]
            {
                "Name", "OwnerId"
            });

        migrationBuilder.CreateIndex(
            "IX_Budget_OwnerId",
            schema: "generic",
            table: "Budget",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            "IX_Participating_BudgetId",
            schema: "generic",
            table: "Participating",
            column: "BudgetId");

        migrationBuilder.CreateIndex(
            "IX_Transaction_BudgetId",
            schema: "generic",
            table: "Transaction",
            column: "BudgetId");

        migrationBuilder.CreateIndex(
            "IX_Transaction_UserId",
            schema: "generic",
            table: "Transaction",
            column: "UserId");

        migrationBuilder.CreateIndex(
            "IX_TransactionVersion_TransactionId",
            schema: "generic",
            table: "TransactionVersion",
            column: "TransactionId");

        migrationBuilder.CreateIndex(
            "IX_User_ActiveBudgetId",
            schema: "generic",
            table: "User",
            column: "ActiveBudgetId");

        migrationBuilder.AddForeignKey(
            "FK_Budget_User_OwnerId",
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
            "FK_Budget_User_OwnerId",
            schema: "generic",
            table: "Budget");

        migrationBuilder.DropTable(
            "Participating",
            "generic");

        migrationBuilder.DropTable(
            "TransactionVersion",
            "generic");

        migrationBuilder.DropTable(
            "Transaction",
            "generic");

        migrationBuilder.DropTable(
            "User",
            "generic");

        migrationBuilder.DropTable(
            "Budget",
            "generic");
    }
}