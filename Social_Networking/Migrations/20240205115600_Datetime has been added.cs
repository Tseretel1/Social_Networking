using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_Networking.Migrations
{
    /// <inheritdoc />
    public partial class Datetimehasbeenadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Post_PostsID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PostsID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostsID",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Post",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "PostsID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PostsID",
                table: "Users",
                column: "PostsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Post_PostsID",
                table: "Users",
                column: "PostsID",
                principalTable: "Post",
                principalColumn: "ID");
        }
    }
}
