using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class FixBlogStyle1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "e9ad8a58-a880-4d13-b20d-3aaf2f18f46d", "c34b2c03-6dc7-4d2e-b2ef-22dfc6bea36f" });

            migrationBuilder.AddColumn<string>(
                name: "BlogStyleName",
                table: "BlogStyle",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "373283d9-aa6e-41c5-98e0-ab8f95f4af2d", "600f814a-14dd-4112-98c7-abc692eb718e", "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "BlogStyle",
                keyColumn: "BlogStyleId",
                keyValue: 1,
                column: "BlogStyleName",
                value: "Soft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "373283d9-aa6e-41c5-98e0-ab8f95f4af2d", "600f814a-14dd-4112-98c7-abc692eb718e" });

            migrationBuilder.DropColumn(
                name: "BlogStyleName",
                table: "BlogStyle");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e9ad8a58-a880-4d13-b20d-3aaf2f18f46d", "c34b2c03-6dc7-4d2e-b2ef-22dfc6bea36f", "Admin", "ADMIN" });
        }
    }
}
