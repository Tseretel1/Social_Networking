using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_Networking.Migrations
{
    /// <inheritdoc />
    public partial class MessagesDatabaseUPdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    SenderUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverID = table.Column<int>(type: "int", nullable: false),
                    ReceiverUserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MessagesUser",
                columns: table => new
                {
                    UsersID = table.Column<int>(type: "int", nullable: false),
                    messagesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesUser", x => new { x.UsersID, x.messagesID });
                    table.ForeignKey(
                        name: "FK_MessagesUser_Messages_messagesID",
                        column: x => x.messagesID,
                        principalTable: "Messages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessagesUser_Users_UsersID",
                        column: x => x.UsersID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessagesUser_messagesID",
                table: "MessagesUser",
                column: "messagesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesUser");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
