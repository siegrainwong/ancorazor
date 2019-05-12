using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AddKeywords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "SiteSetting",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(1407), new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(1419) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(4029), new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(4032) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(2967), new DateTime(2019, 5, 12, 16, 19, 48, 558, DateTimeKind.Local).AddTicks(2971) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 12, 16, 19, 48, 556, DateTimeKind.Local).AddTicks(1980), new DateTime(2019, 5, 12, 16, 19, 48, 556, DateTimeKind.Local).AddTicks(633), new DateTime(2019, 5, 12, 16, 19, 48, 556, DateTimeKind.Local).AddTicks(1494) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "SiteSetting");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(6597), new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(6610) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(9605), new DateTime(2019, 5, 10, 10, 55, 33, 72, DateTimeKind.Local).AddTicks(9608) });

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
    }
}
