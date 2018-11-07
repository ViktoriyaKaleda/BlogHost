using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixBlogStyle4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "bab519de-d700-466f-82b8-8bb2ed7d12ed", "67dccb80-4474-4d75-a5cb-f4d8e69c5562" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "553984b5-1580-45a9-9f54-022791d4cf31", "372e57aa-6454-4dd1-89fc-82a7da464e36", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                columns: new[] { "BackgrounsColor", "SecondColor" },
                values: new object[] { "#eeeeee", "#FFA500" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "553984b5-1580-45a9-9f54-022791d4cf31", "372e57aa-6454-4dd1-89fc-82a7da464e36" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bab519de-d700-466f-82b8-8bb2ed7d12ed", "67dccb80-4474-4d75-a5cb-f4d8e69c5562", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                columns: new[] { "BackgrounsColor", "SecondColor" },
                values: new object[] { "#E6D6C0", "" });
        }
    }
}
