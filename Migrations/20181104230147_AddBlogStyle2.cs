using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class AddBlogStyle2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "14f52813-71e4-48fa-a109-acd21354b92b", "65649be9-1eab-4e6f-a620-ea40f072bbe4" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dd0fe00b-5f68-4d88-b59d-ff9ca9fe5a17", "14a7b910-2388-4293-ae1e-c79b2f4b4715", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "BlogStyle",
                columns: new[] { "BlogStyleId", "BackgrounsColor", "BlogStyleName", "DefaultImagePath", "FisrtColor", "SecondColor", "TitlesFontColor", "TitlesFontName" },
                values: new object[] { 2, "", "Default", "~/images/slider-1.jpg", "#000000", "", "#", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "dd0fe00b-5f68-4d88-b59d-ff9ca9fe5a17", "14a7b910-2388-4293-ae1e-c79b2f4b4715" });

            migrationBuilder.DeleteData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "14f52813-71e4-48fa-a109-acd21354b92b", "65649be9-1eab-4e6f-a620-ea40f072bbe4", "Admin", "ADMIN" });
        }
    }
}
