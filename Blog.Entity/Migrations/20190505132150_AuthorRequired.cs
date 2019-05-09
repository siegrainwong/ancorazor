using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AuthorRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Users",
                table: "Article");

            migrationBuilder.AlterColumn<int>(
                name: "Author",
                table: "Article",
                nullable: false,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValueSql: "((1))");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(9469), new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(7967), new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(8917) });

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Users",
                table: "Article",
                column: "Author",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Users",
                table: "Article");

            migrationBuilder.AlterColumn<int>(
                name: "Author",
                table: "Article",
                nullable: true,
                defaultValueSql: "((1))",
                oldClrType: typeof(int),
                oldDefaultValueSql: "((1))");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 5, 21, 17, 16, 2, DateTimeKind.Local).AddTicks(1179), new DateTime(2019, 5, 5, 21, 17, 16, 1, DateTimeKind.Local).AddTicks(9677), new DateTime(2019, 5, 5, 21, 17, 16, 2, DateTimeKind.Local).AddTicks(622) });

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Users",
                table: "Article",
                column: "Author",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
