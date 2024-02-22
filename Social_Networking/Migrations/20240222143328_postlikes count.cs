using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_Networking.Migrations
{
    /// <inheritdoc />
    public partial class postlikescount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostComments_PostCommentsID",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_PostCommentsID",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostCommentsID",
                table: "Post");

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false),
                    postID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.AddColumn<int>(
                name: "PostCommentsID",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_PostCommentsID",
                table: "Post",
                column: "PostCommentsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_PostComments_PostCommentsID",
                table: "Post",
                column: "PostCommentsID",
                principalTable: "PostComments",
                principalColumn: "ID");
        }
    }
}
