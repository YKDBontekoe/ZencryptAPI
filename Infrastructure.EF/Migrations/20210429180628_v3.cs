using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.EF.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ForumId",
                table: "Post",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ForumType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForumTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forum_ForumType_ForumTypeId",
                        column: x => x.ForumTypeId,
                        principalTable: "ForumType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Forum_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFollowingForum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowingForum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFollowingForum_Forum_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollowingForum_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserHiddenForum",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ForumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHiddenForum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserHiddenForum_Forum_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forum",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHiddenForum_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Post_ForumId",
                table: "Post",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_CreatedByUserId",
                table: "Forum",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Forum_ForumTypeId",
                table: "Forum",
                column: "ForumTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowingForum_ForumId",
                table: "UserFollowingForum",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowingForum_UserId",
                table: "UserFollowingForum",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHiddenForum_ForumId",
                table: "UserHiddenForum",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHiddenForum_UserId",
                table: "UserHiddenForum",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Forum_ForumId",
                table: "Post",
                column: "ForumId",
                principalTable: "Forum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Forum_ForumId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "UserFollowingForum");

            migrationBuilder.DropTable(
                name: "UserHiddenForum");

            migrationBuilder.DropTable(
                name: "Forum");

            migrationBuilder.DropTable(
                name: "ForumType");

            migrationBuilder.DropIndex(
                name: "IX_Post_ForumId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "Post");
        }
    }
}
