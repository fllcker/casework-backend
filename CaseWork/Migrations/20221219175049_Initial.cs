using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CaseWork.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    UserCreatorId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    LastName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Country = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    City = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Horse = table.Column<bool>(type: "boolean", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InviteType = table.Column<int>(type: "integer", nullable: false),
                    LinkHash = table.Column<string>(type: "text", nullable: true),
                    InviteEntityId = table.Column<int>(type: "integer", nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDenied = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    InitiatorId = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invites_Users_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_Users_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleRelations_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRelations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Assignment = table.Column<string>(type: "text", nullable: false),
                    Urgency = table.Column<int>(type: "integer", nullable: false),
                    DeadLine = table.Column<long>(type: "bigint", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: true),
                    AcceptedTime = table.Column<long>(type: "bigint", nullable: false),
                    CompletedTime = table.Column<long>(type: "bigint", nullable: false),
                    EmployerId = table.Column<int>(type: "integer", nullable: false),
                    ExecutorId = table.Column<int>(type: "integer", nullable: false),
                    SubTaskId = table.Column<int>(type: "integer", nullable: true),
                    InviteId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Invites_InviteId",
                        column: x => x.InviteId,
                        principalTable: "Invites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Tasks_SubTaskId",
                        column: x => x.SubTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UserCreatorId",
                table: "Companies",
                column: "UserCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InitiatorId",
                table: "Invites",
                column: "InitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_TargetId",
                table: "Invites",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRelations_RoleId",
                table: "RoleRelations",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRelations_UserId",
                table: "RoleRelations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_EmployerId",
                table: "Tasks",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ExecutorId",
                table: "Tasks",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_InviteId",
                table: "Tasks",
                column: "InviteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SubTaskId",
                table: "Tasks",
                column: "SubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CompanyId",
                table: "Users",
                column: "CompanyId");

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

            migrationBuilder.DropTable(
                name: "RoleRelations");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
