using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class asin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "audio_books",
                newName: "ASIN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ASIN",
                table: "audio_books",
                newName: "ISBN");
        }
    }
}
