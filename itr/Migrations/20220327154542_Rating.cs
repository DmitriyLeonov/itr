using Microsoft.EntityFrameworkCore.Migrations;

namespace itr.Migrations
{
    public partial class Rating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Articles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "ArticleRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleRatings_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRatings_ArticleId",
                table: "ArticleRatings",
                column: "ArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleRatings");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Articles");
        }
    }
}
