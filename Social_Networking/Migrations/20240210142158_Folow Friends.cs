using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_Networking.Migrations
{
    /// <inheritdoc />
    public partial class FolowFriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<int>(type: "int", nullable: false),
                    UserId2 = table.Column<int>(type: "int", nullable: false),
                    UserName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName2 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    RecieverID = table.Column<int>(type: "int", nullable: false),
                    SenderUSername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecieverUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FriendsID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Follows_Friends_FriendsID",
                        column: x => x.FriendsID,
                        principalTable: "Friends",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FriendsID",
                table: "Follows",
                column: "FriendsID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}
