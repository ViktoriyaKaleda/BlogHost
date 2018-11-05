using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class AddBlogStyle1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "bad1b9fd-14f9-4c4a-92e8-b1ce19040d0a", "d935b70c-bcc5-4e05-897e-66191c3ebd65" });

            migrationBuilder.AddColumn<int>(
                name: "BlogStyleId",
                table: "Blog",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogStyle",
                columns: table => new
                {
                    BlogStyleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DefaultImagePath = table.Column<string>(nullable: true),
                    BackgrounsColor = table.Column<string>(nullable: true),
                    FisrtColor = table.Column<string>(nullable: true),
                    SecondColor = table.Column<string>(nullable: true),
                    TitlesFontName = table.Column<string>(nullable: true),
                    TitlesFontColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStyle", x => x.BlogStyleId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e9ad8a58-a880-4d13-b20d-3aaf2f18f46d", "c34b2c03-6dc7-4d2e-b2ef-22dfc6bea36f", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "BlogStyle",
                columns: new[] { "BlogStyleId", "BackgrounsColor", "DefaultImagePath", "FisrtColor", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { 1, "#E6D6C0", "~/images/blog-header6.jpg", "#000000", "", "#763eb6", "font-family: 'Concert One', cursive;" });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_BlogStyleId",
                table: "Blog",
                column: "BlogStyleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog",
                column: "BlogStyleId",
                principalTable: "BlogStyle",
                principalColumn: "BlogStyleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blog_BlogStyle_BlogStyleId",
                table: "Blog");

            migrationBuilder.DropTable(
                name: "BlogStyle");

            migrationBuilder.DropIndex(
                name: "IX_Blog_BlogStyleId",
                table: "Blog");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "e9ad8a58-a880-4d13-b20d-3aaf2f18f46d", "c34b2c03-6dc7-4d2e-b2ef-22dfc6bea36f" });

            migrationBuilder.DropColumn(
                name: "BlogStyleId",
                table: "Blog");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bad1b9fd-14f9-4c4a-92e8-b1ce19040d0a", "d935b70c-bcc5-4e05-897e-66191c3ebd65", "Admin", "ADMIN" });
        }
    }
}
