using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixStyle6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "f5122a7e-73ab-4954-ba28-3a424b2a2b46", "5b263d95-7150-423e-b69d-75f8eac3ff99" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a9ae2659-021a-46c3-9e4c-661c562e8a49", "19576cda-0629-4c09-8f0b-db58574dc4de", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 4,
                column: "TitlesFontName",
                value: "PT Sans, sans-serif");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "a9ae2659-021a-46c3-9e4c-661c562e8a49", "19576cda-0629-4c09-8f0b-db58574dc4de" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5122a7e-73ab-4954-ba28-3a424b2a2b46", "5b263d95-7150-423e-b69d-75f8eac3ff99", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 4,
                column: "TitlesFontName",
                value: "Cormorant, serif");
        }
    }
}
