using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class discs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfDiscs",
                table: "audio_books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfDiscs",
                table: "audio_books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
