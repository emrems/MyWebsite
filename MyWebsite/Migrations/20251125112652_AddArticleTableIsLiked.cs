using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleTableIsLiked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLikes_ArticleId",
                table: "ArticleLikes",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleLikes_Articles_ArticleId",
                table: "ArticleLikes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleLikes_Articles_ArticleId",
                table: "ArticleLikes");

            migrationBuilder.DropIndex(
                name: "IX_ArticleLikes_ArticleId",
                table: "ArticleLikes");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "Articles");
        }
    }
}
