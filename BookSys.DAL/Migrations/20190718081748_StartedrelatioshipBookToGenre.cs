
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookSys.DAL.Migrations
{
    public partial class StartedrelatioshipBookToGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GenreID",
                table: "Books",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreID",
                table: "Books",
                column: "GenreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books",
                column: "GenreID",
                principalTable: "Genres",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_GenreID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_GenreID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "GenreID",
                table: "Books");
        }
    }
}
