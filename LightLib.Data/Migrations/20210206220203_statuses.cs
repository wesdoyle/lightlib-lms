using Microsoft.EntityFrameworkCore.Migrations;

namespace LightLib.Data.Migrations
{
    public partial class statuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The item is lost.", "LOST" },
                    { 2, "The item is in good condition.", "GOOD_CONDITION" },
                    { 3, "The item is in unknown whereabouts and condition.", "UNKNOWN_CONDITION" },
                    { 4, "The item has been destroyed.", "DESTROYED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
