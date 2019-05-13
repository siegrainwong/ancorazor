using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AddRouteMappingForSiteSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteMapping",
                table: "SiteSetting",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(5718), new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(5731) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RouteMapping", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(8368), "date/alias", new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(8371) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(7297), new DateTime(2019, 5, 13, 8, 46, 51, 916, DateTimeKind.Local).AddTicks(7301) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 13, 8, 46, 51, 914, DateTimeKind.Local).AddTicks(6775), new DateTime(2019, 5, 13, 8, 46, 51, 914, DateTimeKind.Local).AddTicks(5402), new DateTime(2019, 5, 13, 8, 46, 51, 914, DateTimeKind.Local).AddTicks(6277) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteMapping",
                table: "SiteSetting");

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
    }
}
