using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixBlogStyle5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "553984b5-1580-45a9-9f54-022791d4cf31", "372e57aa-6454-4dd1-89fc-82a7da464e36" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35f5a44b-d198-40c7-82fb-87df861d4e4d", "4610d087-ec59-4a5c-84ce-af23c5eeab64", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "TitlesFontName",
                value: "Concert One, cursive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "35f5a44b-d198-40c7-82fb-87df861d4e4d", "4610d087-ec59-4a5c-84ce-af23c5eeab64" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "553984b5-1580-45a9-9f54-022791d4cf31", "372e57aa-6454-4dd1-89fc-82a7da464e36", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "TitlesFontName",
                value: "'Concert One', cursive");
        }
    }
}
