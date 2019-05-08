using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.EF.Entity.Migrations
{
    public partial class AddRoleSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "IsEnabled", "Name", "Remark", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(2494), false, true, "Admin", null, new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(2507) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(4009), new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(2648), new DateTime(2019, 5, 7, 16, 6, 56, 318, DateTimeKind.Local).AddTicks(3526) });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Remark", "RoleId", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(4036), false, null, 1, new DateTime(2019, 5, 7, 16, 6, 56, 320, DateTimeKind.Local).AddTicks(4040), 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(2349), new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(906), new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(1860) });
        }
    }
}
