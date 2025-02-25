using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class EmailCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EmailCodes",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCodes", x => new { x.Email, x.Code });
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12df87ed-f081-4e82-bdf0-cf2a305a20d3", null, "Student", "STUDENT" },
                    { "66a81d88-c2f3-4b24-befe-f9965e3e35b1", null, "Teacher", "TEACHER" },
                    { "f83baf9e-7cd1-408e-adb5-5467e8734b53", null, "Department", "DEPARTMENT" },
                    { "f98f7294-36af-452f-81fd-6d8cfdeb8cf5", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailCodes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12df87ed-f081-4e82-bdf0-cf2a305a20d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66a81d88-c2f3-4b24-befe-f9965e3e35b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f83baf9e-7cd1-408e-adb5-5467e8734b53");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f98f7294-36af-452f-81fd-6d8cfdeb8cf5");

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
    }
}
