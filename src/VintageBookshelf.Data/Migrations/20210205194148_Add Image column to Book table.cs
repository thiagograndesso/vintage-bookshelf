using Microsoft.EntityFrameworkCore.Migrations;

namespace VintageBookshelf.Data.Migrations
{
    public partial class AddImagecolumntoBooktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Book",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Book");
        }
    }
}
