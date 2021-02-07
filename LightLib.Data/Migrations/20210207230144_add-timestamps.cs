using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class addtimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Until",
                table: "checkouts",
                newName: "CheckedOutUntil");

            migrationBuilder.RenameColumn(
                name: "Since",
                table: "checkouts",
                newName: "CheckedOutSince");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "patrons",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "patrons",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "library_branches",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "library_branches",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "holds",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "patrons");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "patrons");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "library_branches");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "library_branches");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "holds");

            migrationBuilder.RenameColumn(
                name: "CheckedOutUntil",
                table: "checkouts",
                newName: "Until");

            migrationBuilder.RenameColumn(
                name: "CheckedOutSince",
                table: "checkouts",
                newName: "Since");
        }
    }
}
