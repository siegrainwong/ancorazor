using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ancorazor.Entity.Migrations
{
    public partial class initial : Migration
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
                    RouteMapping = table.Column<string>(nullable: false),
                    Gitment = table.Column<string>(maxLength: 500, nullable: true)
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
                values: new object[,]
                {
                    { 1, "uncategorized", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "Uncategorized", "default category", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) },
                    { 2, "tutorial", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "tutorial", "category for demostration", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "IsEnabled", "Name", "Remark", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), false, true, "Admin", null, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) });

            migrationBuilder.InsertData(
                table: "SiteSetting",
                columns: new[] { "Id", "ArticleTemplate", "Copyright", "CoverUrl", "CreatedAt", "Gitment", "Keywords", "Remark", "RouteMapping", "SiteName", "SubTitle", "Title", "UpdatedAt" },
                values: new object[] { 1, @"---
title: Enter your title here.
category: development
tags:
- dotnet
- dotnet core
---

**Hello world!**", "ancorazor", "upload/default/home-bg.jpg", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), null, null, null, "date/alias", "ancorazor", null, "Ancorazor", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) });

            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "Alias", "CreatedAt", "Name", "Remark", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "markdown", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "markdown", "tag for demostration", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) },
                    { 2, "yaml-front-matter", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "yaml-front-matter", "tag for demostration", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthUpdatedAt", "CreatedAt", "IsDeleted", "LoginName", "Password", "RealName", "Remark", "Status", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), false, "admin", "$SGHASH$V1$10000$RA3Eaw5yszeel1ARIe7iFp2AGWWLd80dAMwr+V4mRcAimv8u", "Admin", null, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) });

            migrationBuilder.InsertData(
                table: "ImageStorage",
                columns: new[] { "Id", "Category", "CreatedAt", "Path", "Remark", "Size", "ThumbPath", "UpdatedAt", "Uploader" },
                values: new object[] { 1, "cover", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "upload/default/post-bg.jpg", "default post cover", 0L, null, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), 1 });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Remark", "RoleId", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), false, null, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), 1 });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Alias", "Category", "CommentCount", "Content", "Cover", "CreatedAt", "Digest", "IsDraft", "Remark", "Title", "UpdatedAt", "ViewCount" },
                values: new object[] { 1, "welcome-to-ancorazor", 2, 0, @"---
title: Welcome to ancorazor!
description: Learn how to write a post.
draft: no
category: tutorial
tags:
- markdown
- yaml-front-matter
date: 2019/6/8
---

Let's take a look at some simple markdown demonstration.
# Headers
## h2
### h3
#### h4
##### h5

# Code
## code block
```C#
public class SiteSettingService
{
	private readonly BlogContext _context;

	public SiteSettingService(BlogContext context)
	{
		_context = context;
	}
}
```
## Inline code
hello `world`!
## More?
Ancorazor's editor based on [EasyMDE](https://github.com/Ionaru/easy-markdown-editor), so there's nothing special on markdown syntax,  If u are not familiar with markdown, this [guide](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) would be a good start.
# YAML front matter
> YFM is an section of valid YAML that is placed at the top of a page and is used for maintaining metadata for the page and its contents.

You need to know this is **required** for every post in the ancorazor.

```yaml
---
title: Welcome to ancorazor!
description: Learn how to write a post.
draft: no
category: tutorial
tags:
- markdown
- yaml-front-matter
date: 2019/6/8
---
```

From top to bottom:
1. title：required.
2. alias：optional, using on url.
3. description：optional, will display below the title.
4. draft：optional, means this article only visible for authorized user,  valid inputs are `true\false\yes\no`.
5. category：optional, categorize an article and can be used on part of the url depends on your site setting, ancorazor doesn't support *multiple categories*.
6. tags：optional
7. date：optional

Basically same as this [documentation](http://assemble.io/docs/YAML-front-matter.html).

# Image upload
## Post cover
Upload your cover to this place, below the editor.
![](http://ww1.sinaimg.cn/large/006bSnAKgy1g3tkwozvfnj30ml031dfn.jpg)
## Insert images to markdown
I would recommend using a chrome extension like [this](https://chrome.google.com/webstore/detail/%E6%96%B0%E6%B5%AA%E5%BE%AE%E5%8D%9A%E5%9B%BE%E5%BA%8A/fdfdnfpdplfbbnemmmoklbfjbhecpnhf?hl=zh-CN).
![](http://ww1.sinaimg.cn/large/006bSnAKgy1g3tkypqgbuj30lu0f2my7.jpg)

Then just paste that markdown to here, pretty simple.

# Tips
Ancorazor has no autosave or something like that, so you'd be better finishing your writing in a .md file first or post this as a draft.

Thanks for reading this guide, hope you can enjoy your writing.", 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "Learn how to write a post.", false, null, "Welcome to ancorazor!", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), 0 });

            migrationBuilder.InsertData(
                table: "Article",
                columns: new[] { "Id", "Alias", "Category", "CommentCount", "Content", "Cover", "CreatedAt", "Digest", "IsDraft", "Remark", "Title", "UpdatedAt", "ViewCount" },
                values: new object[] { 2, "getting-start-with-ancorazor", 2, 0, @"---
title: 欢迎使用ancorazor!
alias: Getting start with ancorazor
description: 本文将向你演示如何写一篇文章.
draft: no
category: tutorial
tags:
- markdown
- yaml-front-matter
date: 2019/6/8
---

先来看一些简单的markdown演示。
# Headers
## h2
### h3
#### h4
##### h5

# Code
## code block
```C#
public class SiteSettingService
{
	private readonly BlogContext _context;

	public SiteSettingService(BlogContext context)
	{
		_context = context;
	}
}
```
## Inline code
hello `world`!
## 还有呢？
Ancorazor使用的编辑器基于[EasyMDE](https://github.com/Ionaru/easy-markdown-editor)，所以基本的markdown语法都是支持的，如果你对markdown还不是很熟悉，建议你可以看下这个[教程](https://www.runoob.com/markdown/md-tutorial.html)。
# YAML front matter
> YFM 用于维护 markdown 页面的元数据的 yaml 格式的内容，所谓的元数据就是指标题、日期、分类和标签等内容，位于 markdown 文件的顶部。

在 ancorazor 中，**每篇**文章都需要`yaml-front-matter`提供必须的内容。

```yaml
---
title: Welcome to ancorazor!
alias: Getting start with ancorazor
description: Learn how to write a post.
draft: no
category: tutorial
tags:
- markdown
- yaml-front-matter
date: 2019/6/8
---
```

从上到下依次为：
1. title 标题：必填
2. alias 别名：可选，用于 Url 的显示，中文会自动转换为拼音，不填写的话会从 title 上取
3. description 描述：可选，会显示在列表和文章的标题下方
4. draft 草稿：可选，草稿只有你自己能看见，有效值为`true\false\yes\no`
5. category 分类：可选，为文章分类，且可在站点配置中将其作为 Url 的一部分，注意 ancorazor 只支持单分类不支持多分类
6. tags 标签：可选
7. date 文章日期：可选

基本跟该[文档](http://assemble.io/docs/YAML-front-matter.html)是一样的。
# 图片上传
## 文章封面
将你的封面拖、上传到位于编辑器下方的这个位置即可。
![](http://ww1.sinaimg.cn/large/006bSnAKgy1g3tkwozvfnj30ml031dfn.jpg)
## 在文章中插入图片
推荐你使用类似于这样的[图床插件](https://chrome.google.com/webstore/detail/%E6%96%B0%E6%B5%AA%E5%BE%AE%E5%8D%9A%E5%9B%BE%E5%BA%8A/fdfdnfpdplfbbnemmmoklbfjbhecpnhf?hl=zh-CN).
![](http://ww1.sinaimg.cn/large/006bSnAKgy1g3tkypqgbuj30lu0f2my7.jpg)

然后直接把 markdown 粘贴进来即可。

# 提示
Ancorazor **没有提供类似于自动保存的功能**，所以建议在 .md 文件里或者其他 markdown 编辑器中写好后再粘贴进来发布。

感谢阅读本篇教程，祝您写作愉快~", 1, new DateTime(2019, 6, 23, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), "本文将向你演示如何写一篇文章.", false, null, "欢迎使用ancorazor!", new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), 0 });

            migrationBuilder.InsertData(
                table: "ArticleTags",
                columns: new[] { "Id", "Article", "CreatedAt", "Remark", "Tag", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), null, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) },
                    { 2, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), null, 2, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) },
                    { 3, 2, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), null, 1, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) },
                    { 4, 2, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597), null, 2, new DateTime(2019, 6, 24, 22, 23, 37, 348, DateTimeKind.Local).AddTicks(3597) }
                });

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
