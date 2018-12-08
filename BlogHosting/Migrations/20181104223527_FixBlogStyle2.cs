using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixBlogStyle2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "373283d9-aa6e-41c5-98e0-ab8f95f4af2d", "600f814a-14dd-4112-98c7-abc692eb718e" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "14f52813-71e4-48fa-a109-acd21354b92b", "65649be9-1eab-4e6f-a620-ea40f072bbe4", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog",
                column: "BlogStyleId",
                principalTable: "BlogStyle",
                principalColumn: "BlogStyleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "14f52813-71e4-48fa-a109-acd21354b92b", "65649be9-1eab-4e6f-a620-ea40f072bbe4" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "373283d9-aa6e-41c5-98e0-ab8f95f4af2d", "600f814a-14dd-4112-98c7-abc692eb718e", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog",
                column: "BlogStyleId",
                principalTable: "BlogStyle",
                principalColumn: "BlogStyleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
