using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixStyles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "a9ae2659-021a-46c3-9e4c-661c562e8a49", "19576cda-0629-4c09-8f0b-db58574dc4de" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "918a43cb-91f9-4b2d-af9a-681f0b7fe4b7", "3ae6f288-059f-4748-8817-279631402c53", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "DefaultImagePath",
                value: "/images/slider-1.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 2,
                column: "DefaultImagePath",
                value: "/images/blog-header6.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 3,
                column: "DefaultImagePath",
                value: "/images/blog-header3.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 4,
                column: "DefaultImagePath",
                value: "/images/blog-header4.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "918a43cb-91f9-4b2d-af9a-681f0b7fe4b7", "3ae6f288-059f-4748-8817-279631402c53" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a9ae2659-021a-46c3-9e4c-661c562e8a49", "19576cda-0629-4c09-8f0b-db58574dc4de", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "DefaultImagePath",
                value: "~/images/slider-1.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 2,
                column: "DefaultImagePath",
                value: "~/images/blog-header6.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 3,
                column: "DefaultImagePath",
                value: "~/images/blog-header3.jpg");

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 4,
                column: "DefaultImagePath",
                value: "~/images/blog-header4.jpg");
        }
    }
}
