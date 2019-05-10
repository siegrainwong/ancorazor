using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class SiteSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Article_Title",
                table: "Article");

            migrationBuilder.CreateTable(
                name: "SiteSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(nullable: false, defaultValue: "Ancore"),
                    SubTitle = table.Column<string>(nullable: true),
                    SiteName = table.Column<string>(nullable: false, defaultValue: "ancore"),
                    Copyright = table.Column<string>(nullable: false, defaultValue: "ancore"),
                    CoverUrl = table.Column<string>(nullable: false, defaultValue: "assets/img/write-bg.jpg"),
                    ArticleTemplate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetting", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Article_Alias",
                table: "Article",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_Title",
                table: "Article",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSetting");

            migrationBuilder.DropIndex(
                name: "IX_Article_Alias",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_Title",
                table: "Article");

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

            migrationBuilder.CreateIndex(
                name: "IX_Article_Title",
                table: "Article",
                column: "Id",
                unique: true);
        }
    }
}
