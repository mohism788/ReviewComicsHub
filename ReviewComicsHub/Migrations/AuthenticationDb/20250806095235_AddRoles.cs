using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComicsAPI.Migrations.AuthenticationDb
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "08014d18-e4c5-4b44-9e1a-859626270bc4", "08014d18-e4c5-4b44-9e1a-859626270bc4", "NormalUser", "NORMALUSER" },
                    { "e06d1c43-60d0-4d74-b69b-f0a15c6c636a", "e06d1c43-60d0-4d74-b69b-f0a15c6c636a", "Moderator", "MODERATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08014d18-e4c5-4b44-9e1a-859626270bc4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e06d1c43-60d0-4d74-b69b-f0a15c6c636a");
        }
    }
}
