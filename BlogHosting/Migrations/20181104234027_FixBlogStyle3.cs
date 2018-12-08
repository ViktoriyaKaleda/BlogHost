using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixBlogStyle3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "dd0fe00b-5f68-4d88-b59d-ff9ca9fe5a17", "14a7b910-2388-4293-ae1e-c79b2f4b4715" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bab519de-d700-466f-82b8-8bb2ed7d12ed", "67dccb80-4474-4d75-a5cb-f4d8e69c5562", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "TitlesFontName",
                value: "'Concert One', cursive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "bab519de-d700-466f-82b8-8bb2ed7d12ed", "67dccb80-4474-4d75-a5cb-f4d8e69c5562" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd0fe00b-5f68-4d88-b59d-ff9ca9fe5a17", "14a7b910-2388-4293-ae1e-c79b2f4b4715", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "TitlesFontName",
                value: "font-family: 'Concert One', cursive;");
        }
    }
}
