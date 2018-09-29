using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Data.Migrations
{
    public partial class FixCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_ChildCommentCommentId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "ChildCommentCommentId",
                table: "Comment",
                newName: "CommentId1");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ChildCommentCommentId",
                table: "Comment",
                newName: "IX_Comment_CommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_CommentId1",
                table: "Comment",
                column: "CommentId1",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_CommentId1",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "CommentId1",
                table: "Comment",
                newName: "ChildCommentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_CommentId1",
                table: "Comment",
                newName: "IX_Comment_ChildCommentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_ChildCommentCommentId",
                table: "Comment",
                column: "ChildCommentCommentId",
                principalTable: "Comment",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
