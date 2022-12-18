using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseWork.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCreatorId",
                table: "Companies",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserCreatorId",
                table: "Companies",
                column: "UserCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Users_UserCreatorId",
                table: "Companies",
                column: "UserCreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Users_UserCreatorId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_UserCreatorId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UserCreatorId",
                table: "Companies");
        }
    }
}
