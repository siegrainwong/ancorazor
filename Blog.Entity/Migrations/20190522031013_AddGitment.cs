using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AddGitment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gitment",
                table: "SiteSetting",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "ImageStorage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArticleTemplate", "CreatedAt", "UpdatedAt" },
                values: new object[] { @"---
title: Enter your title here.
category: development
tags:
- dotnet
- dotnet core
---

**Hello world!**", new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980), new DateTime(2019, 5, 22, 11, 10, 13, 74, DateTimeKind.Local).AddTicks(5980) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gitment",
                table: "SiteSetting");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 15, 20, 24, 59, 960, DateTimeKind.Local).AddTicks(2625), new DateTime(2019, 5, 15, 20, 24, 59, 960, DateTimeKind.Local).AddTicks(2635) });

            migrationBuilder.UpdateData(
                table: "ImageStorage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(5756), new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(5759) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 15, 20, 24, 59, 943, DateTimeKind.Local).AddTicks(9327), new DateTime(2019, 5, 15, 20, 24, 59, 943, DateTimeKind.Local).AddTicks(9343) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArticleTemplate", "CreatedAt", "UpdatedAt" },
                values: new object[] { null, new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(2194), new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(2197) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(1004), new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(1007) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 15, 20, 24, 59, 941, DateTimeKind.Local).AddTicks(8401), new DateTime(2019, 5, 15, 20, 24, 59, 936, DateTimeKind.Local).AddTicks(2725), new DateTime(2019, 5, 15, 20, 24, 59, 941, DateTimeKind.Local).AddTicks(7867) });
        }
    }
}
