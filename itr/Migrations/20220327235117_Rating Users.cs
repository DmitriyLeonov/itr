using Microsoft.EntityFrameworkCore.Migrations;

namespace itr.Migrations
{
    public partial class RatingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleRatings_Articles_ArticleId",
                table: "ArticleRatings");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "ArticleRatings",
                newName: "articleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleRatings_ArticleId",
                table: "ArticleRatings",
                newName: "IX_ArticleRatings_articleId");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "ArticleRatings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "articleId",
                table: "ArticleRatings",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleRatings_Articles_articleId",
                table: "ArticleRatings",
                column: "articleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleRatings_Articles_articleId",
                table: "ArticleRatings");

            migrationBuilder.RenameColumn(
                name: "articleId",
                table: "ArticleRatings",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleRatings_articleId",
                table: "ArticleRatings",
                newName: "IX_ArticleRatings_ArticleId");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ArticleRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "ArticleRatings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleRatings_Articles_ArticleId",
                table: "ArticleRatings",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
