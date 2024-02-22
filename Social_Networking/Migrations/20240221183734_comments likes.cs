using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_Networking.Migrations
{
    /// <inheritdoc />
    public partial class commentslikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostCommentsID",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.ID);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_PostComments_PostCommentsID",
                table: "Post");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_Post_PostCommentsID",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostCommentsID",
                table: "Post");
        }
    }
}
