using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class addoneone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_periodicals_AssetId",
                table: "periodicals");

            migrationBuilder.DropIndex(
                name: "IX_dvds_AssetId",
                table: "dvds");

            migrationBuilder.DropIndex(
                name: "IX_books_AssetId",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_audio_cds_AssetId",
                table: "audio_cds");

            migrationBuilder.DropIndex(
                name: "IX_audio_books_AssetId",
                table: "audio_books");

            migrationBuilder.CreateIndex(
                name: "IX_periodicals_AssetId",
                table: "periodicals",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dvds_AssetId",
                table: "dvds",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_books_AssetId",
                table: "books",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audio_cds_AssetId",
                table: "audio_cds",
                column: "AssetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audio_books_AssetId",
                table: "audio_books",
                column: "AssetId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_periodicals_AssetId",
                table: "periodicals");

            migrationBuilder.DropIndex(
                name: "IX_dvds_AssetId",
                table: "dvds");

            migrationBuilder.DropIndex(
                name: "IX_books_AssetId",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_audio_cds_AssetId",
                table: "audio_cds");

            migrationBuilder.DropIndex(
                name: "IX_audio_books_AssetId",
                table: "audio_books");

            migrationBuilder.CreateIndex(
                name: "IX_periodicals_AssetId",
                table: "periodicals",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_dvds_AssetId",
                table: "dvds",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_books_AssetId",
                table: "books",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_audio_cds_AssetId",
                table: "audio_cds",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_audio_books_AssetId",
                table: "audio_books",
                column: "AssetId");
        }
    }
}
