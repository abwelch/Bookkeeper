using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookkeeper.Migrations
{
    public partial class EditTransactionDateAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedDate",
                schema: "Recording",
                table: "JournalTransactions");

            migrationBuilder.DropColumn(
                name: "RecordedTime",
                schema: "Recording",
                table: "JournalTransactions");

            migrationBuilder.AddColumn<string>(
                name: "RecordedDateTime",
                schema: "Recording",
                table: "JournalTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedDateTime",
                schema: "Recording",
                table: "JournalTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordedDate",
                schema: "Recording",
                table: "JournalTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "RecordedTime",
                schema: "Recording",
                table: "JournalTransactions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
