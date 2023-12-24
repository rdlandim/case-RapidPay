using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.RapidPay.Migrations
{
    /// <inheritdoc />
    public partial class SeedingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { 1, "user@test.com", "Test User", "AA68CC546E2758BB0130E335A8AEC4785F684A6666DD96E407610D2B4CF3EEF3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
