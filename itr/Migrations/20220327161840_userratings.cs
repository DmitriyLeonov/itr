using Microsoft.EntityFrameworkCore.Migrations;

namespace itr.Migrations
{
    public partial class userratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ArticleRatings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ArticleRatings");
        }
    }
}
