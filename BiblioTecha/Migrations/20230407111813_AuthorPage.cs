using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiblioTecha.Migrations
{
    public partial class AuthorPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "BookModel");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "BookModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuthorModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookModel_AuthorId",
                table: "BookModel",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookModel_AuthorModel_AuthorId",
                table: "BookModel",
                column: "AuthorId",
                principalTable: "AuthorModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookModel_AuthorModel_AuthorId",
                table: "BookModel");

            migrationBuilder.DropTable(
                name: "AuthorModel");

            migrationBuilder.DropIndex(
                name: "IX_BookModel_AuthorId",
                table: "BookModel");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "BookModel");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BookModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
