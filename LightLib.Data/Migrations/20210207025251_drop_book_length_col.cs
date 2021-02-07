using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class drop_book_length_col : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthMinutes",
                table: "books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LengthMinutes",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
