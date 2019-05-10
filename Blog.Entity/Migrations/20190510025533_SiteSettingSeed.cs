using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class SiteSettingSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(6597), new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(6610) });

            migrationBuilder.InsertData(
                table: "SiteSetting",
                columns: new[] { "Id", "ArticleTemplate", "Copyright", "CoverUrl", "CreatedAt", "Remark", "SiteName", "SubTitle", "Title", "UpdatedAt" },
                values: new object[] { 1, null, "ancore", "assets/img/write-bg.jpg", new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(9605), null, "ancore", null, "Ancore", new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(9608) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(8388), new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(8391) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 55, 33, 70, DateTimeKind.Local).AddTicks(6482), new DateTime(2019, 5, 10, 10, 55, 33, 70, DateTimeKind.Local).AddTicks(5108), new DateTime(2019, 5, 10, 10, 55, 33, 70, DateTimeKind.Local).AddTicks(5988) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 48, 23, 648, DateTimeKind.Local).AddTicks(3712), new DateTime(2019, 5, 10, 10, 48, 23, 648, DateTimeKind.Local).AddTicks(3723) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 48, 23, 648, DateTimeKind.Local).AddTicks(5323), new DateTime(2019, 5, 10, 10, 48, 23, 648, DateTimeKind.Local).AddTicks(5326) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 48, 23, 646, DateTimeKind.Local).AddTicks(4594), new DateTime(2019, 5, 10, 10, 48, 23, 646, DateTimeKind.Local).AddTicks(3213), new DateTime(2019, 5, 10, 10, 48, 23, 646, DateTimeKind.Local).AddTicks(4094) });
        }
    }
}
