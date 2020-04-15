using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookkeeper.Migrations
{
    public partial class UpdateJournalEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDebit",
                schema: "Recording",
                table: "JournalEntries");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Recording",
                table: "JournalEntries",
                newName: "DebitAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditAmount",
                schema: "Recording",
                table: "JournalEntries",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditAmount",
                schema: "Recording",
                table: "JournalEntries");

            migrationBuilder.RenameColumn(
                name: "DebitAmount",
                schema: "Recording",
                table: "JournalEntries",
                newName: "Amount");

            migrationBuilder.AddColumn<bool>(
                name: "IsDebit",
                schema: "Recording",
                table: "JournalEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
