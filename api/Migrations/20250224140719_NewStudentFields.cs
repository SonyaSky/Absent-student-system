using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class NewStudentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2df69a52-4eeb-4cbe-977e-d60b3f0a2270");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "785df9ce-f07c-4503-be0b-483f1b0a83b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e36ff1f-b11e-43eb-aaed-d5292fe840cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f99b31c7-fa54-4b33-b240-a7d54787ca2d");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Surname");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8363ab19-fe7b-423a-ab97-d5b1795d4616", null, "Department", "DEPARTMENT" },
                    { "aa75ebe2-27aa-4ac9-bf50-8a6160dbf958", null, "Teacher", "TEACHER" },
                    { "f4d568dc-6189-47c7-ba1a-87949770e1e7", null, "Admin", "ADMIN" },
                    { "f8129b05-620e-4575-bf06-a1c265b8341c", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8363ab19-fe7b-423a-ab97-d5b1795d4616");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa75ebe2-27aa-4ac9-bf50-8a6160dbf958");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4d568dc-6189-47c7-ba1a-87949770e1e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8129b05-620e-4575-bf06-a1c265b8341c");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2df69a52-4eeb-4cbe-977e-d60b3f0a2270", null, "Admin", "ADMIN" },
                    { "785df9ce-f07c-4503-be0b-483f1b0a83b0", null, "Student", "STUDENT" },
                    { "9e36ff1f-b11e-43eb-aaed-d5292fe840cb", null, "Department", "DEPARTMENT" },
                    { "f99b31c7-fa54-4b33-b240-a7d54787ca2d", null, "Teacher", "TEACHER" }
                });
        }
    }
}
