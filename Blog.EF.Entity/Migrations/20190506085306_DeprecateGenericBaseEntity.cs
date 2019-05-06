using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.EF.Entity.Migrations
{
    public partial class DeprecateGenericBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey("PK_OperationLog", "OperationLog");
            migrationBuilder.DropColumn("Id", "OperationLog");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OperationLog",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey("PK_OperationLog", "OperationLog", "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(2349), new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(906), new DateTime(2019, 5, 6, 16, 53, 5, 874, DateTimeKind.Local).AddTicks(1860) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OperationLog",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(9469), new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(7967), new DateTime(2019, 5, 5, 21, 21, 50, 41, DateTimeKind.Local).AddTicks(8917) });
        }
    }
}
