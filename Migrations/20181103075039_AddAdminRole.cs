using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHosting.Migrations
{
    public partial class AddAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "67775d0a-f281-4afe-a474-aaa7e7b77e64", "4f2fce6b-e53a-48f5-8f3b-21cf99c6abe9", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { "67775d0a-f281-4afe-a474-aaa7e7b77e64", "4f2fce6b-e53a-48f5-8f3b-21cf99c6abe9" });
        }
    }
}
