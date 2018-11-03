using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixTypoInTagModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Post_PostId1",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_PostId1",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Tag",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Tag_PostId",
                table: "Tag",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Post_PostId",
                table: "Tag",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Post_PostId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_PostId",
                table: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "Tag",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "Tag",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_PostId1",
                table: "Tag",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Post_PostId1",
                table: "Tag",
                column: "PostId1",
                principalTable: "Post",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
