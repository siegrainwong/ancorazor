using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AliasForCatsAndTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Tag",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Category",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 9, 20, 22, 47, 391, DateTimeKind.Local).AddTicks(1122), new DateTime(2019, 5, 9, 20, 22, 47, 391, DateTimeKind.Local).AddTicks(1136) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 9, 20, 22, 47, 391, DateTimeKind.Local).AddTicks(2707), new DateTime(2019, 5, 9, 20, 22, 47, 391, DateTimeKind.Local).AddTicks(2710) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 9, 20, 22, 47, 389, DateTimeKind.Local).AddTicks(1693), new DateTime(2019, 5, 9, 20, 22, 47, 389, DateTimeKind.Local).AddTicks(302), new DateTime(2019, 5, 9, 20, 22, 47, 389, DateTimeKind.Local).AddTicks(1202) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(2494), new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(2507) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(4036), new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(4040) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(4009), new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(2648), new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(3526) });
        }
    }
}
