using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Alias = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    LoginName = table.Column<string>(maxLength: 200, nullable: true),
                    Area = table.Column<string>(maxLength: 200, nullable: true),
                    Controller = table.Column<string>(maxLength: 200, nullable: true),
                    Action = table.Column<string>(maxLength: 200, nullable: true),
                    IPAddress = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Title = table.Column<string>(nullable: false),
                    SubTitle = table.Column<string>(nullable: true),
                    SiteName = table.Column<string>(nullable: false),
                    Copyright = table.Column<string>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    CoverUrl = table.Column<string>(nullable: false),
                    ArticleTemplate = table.Column<string>(nullable: true),
                    RouteMapping = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Alias = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    LoginName = table.Column<string>(maxLength: 60, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: false),
                    RealName = table.Column<string>(maxLength: 60, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    AuthUpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageStorage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Uploader = table.Column<int>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    Path = table.Column<string>(maxLength: 500, nullable: false),
                    ThumbPath = table.Column<string>(maxLength: 500, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageStorage_Users",
                        column: x => x.Uploader,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.UserRole_dbo.Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.UserRole_dbo.sysUserInfo_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Cover = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Category = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Author = table.Column<int>(nullable: false, defaultValueSql: "((1))"),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false),
                    Digest = table.Column<string>(maxLength: 500, nullable: true),
                    Alias = table.Column<string>(maxLength: 256, nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    CommentCount = table.Column<int>(nullable: false),
                    IsDraft = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Users",
                        column: x => x.Author,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Article_Category",
                        column: x => x.Category,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Article_ImageStorage",
                        column: x => x.Cover,
                        principalTable: "ImageStorage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Remark = table.Column<string>(maxLength: 256, nullable: true),
                    Article = table.Column<int>(nullable: false),
                    Tag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Article",
                        column: x => x.Article,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tag",
                        column: x => x.Tag,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Alias", "CreatedAt", "Name", "Remark", "UpdatedAt" },
                values: new object[] { 1, "uncategorized", new DateTime(2019, 5, 15, 20, 24, 59, 960, DateTimeKind.Local).AddTicks(2625), "Uncategorized", "default category", new DateTime(2019, 5, 15, 20, 24, 59, 960, DateTimeKind.Local).AddTicks(2635) });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "IsEnabled", "Name", "Remark", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2019, 5, 15, 20, 24, 59, 943, DateTimeKind.Local).AddTicks(9327), false, true, "Admin", null, new DateTime(2019, 5, 15, 20, 24, 59, 943, DateTimeKind.Local).AddTicks(9343) });

            migrationBuilder.InsertData(
                table: "SiteSetting",
                columns: new[] { "Id", "ArticleTemplate", "Copyright", "CoverUrl", "CreatedAt", "Keywords", "Remark", "RouteMapping", "SiteName", "SubTitle", "Title", "UpdatedAt" },
                values: new object[] { 1, null, "ancorazor", "upload/default/home-bg.jpg", new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(2194), null, null, "date/alias", "ancorazor", null, "Ancorazor", new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(2197) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthUpdatedAt", "CreatedAt", "IsDeleted", "LoginName", "Password", "RealName", "Remark", "Status", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2019, 5, 15, 20, 24, 59, 941, DateTimeKind.Local).AddTicks(8401), new DateTime(2019, 5, 15, 20, 24, 59, 936, DateTimeKind.Local).AddTicks(2725), false, "admin", "$SGHASH$V1$10000$RA3Eaw5yszeel1ARIe7iFp2AGWWLd80dAMwr+V4mRcAimv8u", "Admin", null, 1, new DateTime(2019, 5, 15, 20, 24, 59, 941, DateTimeKind.Local).AddTicks(7867) });

            migrationBuilder.InsertData(
                table: "ImageStorage",
                columns: new[] { "Id", "Category", "CreatedAt", "Path", "Remark", "Size", "ThumbPath", "UpdatedAt", "Uploader" },
                values: new object[] { 1, "cover", new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(5756), "upload/default/post-bg.jpg", "default post cover", 0L, null, new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(5759), 1 });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Remark", "RoleId", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(1004), false, null, 1, new DateTime(2019, 5, 15, 20, 24, 59, 944, DateTimeKind.Local).AddTicks(1007), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Article_Alias",
                table: "Article",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_Author",
                table: "Article",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Category",
                table: "Article",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Cover",
                table: "Article",
                column: "Cover");

            migrationBuilder.CreateIndex(
                name: "IX_Article_Title",
                table: "Article",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_Tag",
                table: "ArticleTags",
                column: "Tag");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_Article_Tag",
                table: "ArticleTags",
                columns: new[] { "Article", "Tag" });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Alias",
                table: "Category",
                column: "Alias",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name_Id",
                table: "Category",
                columns: new[] { "Name", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ImageStorage_Path",
                table: "ImageStorage",
                column: "Path",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageStorage_ThumbPath",
                table: "ImageStorage",
                column: "ThumbPath",
                unique: true,
                filter: "[ThumbPath] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ImageStorage_Uploader",
                table: "ImageStorage",
                column: "Uploader");

            migrationBuilder.CreateIndex(
                name: "Role_Name_uindex",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name_Id",
                table: "Tag",
                columns: new[] { "Name", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "OperationLog");

            migrationBuilder.DropTable(
                name: "SiteSetting");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ImageStorage");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
