using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseWork.Migrations
{
    /// <inheritdoc />
    public partial class _8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InviteId",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_InviteId",
                table: "Tasks",
                column: "InviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Invites_InviteId",
                table: "Tasks",
                column: "InviteId",
                principalTable: "Invites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Invites_InviteId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_InviteId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "InviteId",
                table: "Tasks");
        }
    }
}
