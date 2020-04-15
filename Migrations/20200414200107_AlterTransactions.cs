using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookkeeper.Migrations
{
    public partial class AlterTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                schema: "Recording",
                table: "JournalEntries");

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitAmount",
                schema: "Recording",
                table: "JournalEntries",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditAmount",
                schema: "Recording",
                table: "JournalEntries",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<bool>(
                name: "DebitBalance",
                schema: "Recording",
                table: "JournalEntries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebitBalance",
                schema: "Recording",
                table: "JournalEntries");

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitAmount",
                schema: "Recording",
                table: "JournalEntries",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditAmount",
                schema: "Recording",
                table: "JournalEntries",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                schema: "Recording",
                table: "JournalEntries",
                type: "varchar(30)",
                unicode: false,
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
