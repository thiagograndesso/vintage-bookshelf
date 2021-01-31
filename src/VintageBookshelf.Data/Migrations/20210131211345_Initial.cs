using Microsoft.EntityFrameworkCore.Migrations;

namespace VintageBookshelf.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    BirthDate = table.Column<string>(type: "DATETIMEOFFSET", nullable: false),
                    Biography = table.Column<string>(type: "VARCHAR(1000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookshelf",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Address = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    City = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookshelf", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Publisher = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "VARCHAR(1000)", nullable: false),
                    AuthorId = table.Column<long>(type: "INTEGER", nullable: false),
                    BookshelfId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Book_Bookshelf_BookshelfId",
                        column: x => x.BookshelfId,
                        principalTable: "Bookshelf",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookshelfId",
                table: "Book",
                column: "BookshelfId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Bookshelf");
        }
    }
}
