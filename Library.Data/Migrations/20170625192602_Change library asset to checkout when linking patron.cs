using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Data.Migrations
{
    public partial class Changelibraryassettocheckoutwhenlinkingpatron : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_LibraryCards_LibraryCardId",
                table: "LibraryAssets");

            migrationBuilder.DropIndex(
                name: "IX_LibraryAssets_LibraryCardId",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "LibraryCardId",
                table: "LibraryAssets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LibraryCardId",
                table: "LibraryAssets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_LibraryCardId",
                table: "LibraryAssets",
                column: "LibraryCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_LibraryCards_LibraryCardId",
                table: "LibraryAssets",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
