using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class AddMoreStyles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "35f5a44b-d198-40c7-82fb-87df861d4e4d", "4610d087-ec59-4a5c-84ce-af23c5eeab64" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5122a7e-73ab-4954-ba28-3a424b2a2b46", "5b263d95-7150-423e-b69d-75f8eac3ff99", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                columns: new[] { "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { "", "Default", "~/images/slider-1.jpg", "", "#", "" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 2,
                columns: new[] { "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { "#eeeeee", "Soft", "~/images/blog-header6.jpg", "#FFA500", "#763eb6", "Concert One, cursive" });

            migrationBuilder.InsertData(
                table: "BlogStyle",
                columns: new[] { "BlogStyleId", "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "FisrtColor", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[,]
                {
                    { 3, "#F9F2FA", "Gentle", "~/images/blog-header3.jpg", "#000000", "#B6798F", "#763eb6", "Cormorant, serif" },
                    { 4, "#C7C0E0", "Ultraviolet", "~/images/blog-header4.jpg", "#000000", "#001b8a", "#763eb6", "Cormorant, serif" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "f5122a7e-73ab-4954-ba28-3a424b2a2b46", "5b263d95-7150-423e-b69d-75f8eac3ff99" });

            migrationBuilder.DeleteData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35f5a44b-d198-40c7-82fb-87df861d4e4d", "4610d087-ec59-4a5c-84ce-af23c5eeab64", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                columns: new[] { "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { "#eeeeee", "Soft", "~/images/blog-header6.jpg", "#FFA500", "#763eb6", "Concert One, cursive" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 2,
                columns: new[] { "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { "", "Default", "~/images/slider-1.jpg", "", "#", "" });
        }
    }
}
