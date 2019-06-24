using Ancorazor.API.Common;
using Ancorazor.API.Common.Constants;
using Ancorazor.Entity.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Ancorazor.Entity
{
    public class BlogContext : DbContext
    {
        public BlogContext() { }

        public BlogContext(DbContextOptions<BlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleTags> ArticleTags { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<OperationLog> OperationLog { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<SiteSetting> SiteSetting { get; set; }
        public virtual DbSet<ImageStorage> ImageStorage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                    .AddJsonFile("appsettings.Development.json", optional: false).Build();

                // for db migration
                optionsBuilder.UseSqlServer(config[$"{nameof(DbConfiguration)}:{nameof(DbConfiguration.ConnectionString)}"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            #region Structures

            // set default value of base entity
            foreach (var entityType in builder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType).Property("CreatedAt")
                    .HasDefaultValueSql("getdate()");
                builder.Entity(entityType.ClrType).Property("UpdatedAt")
                    .HasDefaultValueSql("getdate()");
            }

            builder.Entity<Article>(entity =>
            {
                entity.HasIndex(e => e.Alias)
                    .HasName("IX_Article_Alias")
                    .IsUnique();

                entity.HasIndex(e => e.Title)
                    .HasName("IX_Article_Title")
                    .IsUnique();

                entity.Property(e => e.Author).HasDefaultValueSql("((1))");
                entity.Property(e => e.Cover).HasDefaultValueSql("((1))");
                entity.Property(e => e.Category).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Article_Users");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK_Article_Category");

                entity.HasOne(d => d.ImageStorageNavigation)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.Cover)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Article_ImageStorage");
            });

            builder.Entity<ArticleTags>(entity =>
            {
                entity.HasIndex(e => new { e.Article, e.Tag });

                entity.HasOne(d => d.ArticleNavigation)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.Article)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ArticleTags_Article");

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.Tag)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ArticleTags_Tag");
            });

            builder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_Category_Name")
                    .IsUnique();

                entity.HasIndex(e => e.Alias)
                    .HasName("IX_Category_Alias")
                    .IsUnique();

                entity.HasIndex(e => new { e.Name, e.Id });
            });

            builder.Entity<OperationLog>(entity =>
            {
                entity.Property(e => e.IPAddress).IsUnicode(false);
            });

            builder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("Role_Name_uindex")
                    .IsUnique();
            });

            builder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Id });
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_dbo.UserRole_dbo.Role_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.UserRole_dbo.sysUserInfo_UserId");
            });

            builder.Entity<Users>(entity =>
            {
                entity.Property(e => e.AuthUpdatedAt).HasDefaultValueSql("(getdate())");
            });

            builder.Entity<SiteSetting>();

            builder.Entity<ImageStorage>(entity =>
            {
                entity.HasIndex(e => e.Path)
                    .HasName("IX_ImageStorage_Path")
                    .IsUnique();

                entity.HasIndex(e => e.ThumbPath)
                    .HasName("IX_ImageStorage_ThumbPath")
                    .IsUnique();

                entity.HasOne(d => d.UploaderNavigation)
                    .WithMany(p => p.ImageStorage)
                    .HasForeignKey(d => d.Uploader)
                    .HasConstraintName("FK_ImageStorage_Users");
            });

            #endregion

            #region Seeds
            var now = DateTime.Now;

            builder.Entity<Users>().HasData(new Users
            {
                Id = 1,
                LoginName = "admin",
                // 123456
                Password = "$SGHASH$V1$10000$RA3Eaw5yszeel1ARIe7iFp2AGWWLd80dAMwr+V4mRcAimv8u",
                RealName = "Admin",
                Status = 1,
                CreatedAt = now,
                UpdatedAt = now,
                AuthUpdatedAt = now,
                Remark = null,
                IsDeleted = false
            });
            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin",
                IsDeleted = false,
                IsEnabled = true,
                CreatedAt = now,
                UpdatedAt = now,
                Remark = null
            });
            builder.Entity<UserRole>().HasData(new UserRole
            {
                Id = 1,
                RoleId = 1,
                UserId = 1,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false,
                Remark = null
            });
            builder.Entity<SiteSetting>().HasData(new SiteSetting
            {
                Id = 1,
                CreatedAt = now,
                UpdatedAt = now,
                Title = "Ancorazor",
                SiteName = "ancorazor",
                Copyright = "ancorazor",
                CoverUrl = "upload/default/home-bg.jpg",
                RouteMapping = "date/alias",
                ArticleTemplate = @"---
title: Enter your title here.
category: development
tags:
- dotnet
- dotnet core
---

**Hello world!**",
                Remark = null
            });
            builder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = Constants.Article.DefaultCategoryName,
                Alias = UrlHelper.ToUrlSafeString(Constants.Article.DefaultCategoryName),
                CreatedAt = now,
                UpdatedAt = now,
                Remark = "default category"
            },
            new Category
            {
                Id = 2,
                Name = "tutorial",
                Alias = UrlHelper.ToUrlSafeString("tutorial"),
                CreatedAt = now,
                UpdatedAt = now,
                Remark = "category for demostration"
            });
            builder.Entity<Tag>().HasData(new Tag
            {
                Id = 1,
                Name = "markdown",
                Alias = UrlHelper.ToUrlSafeString("markdown"),
                CreatedAt = now,
                UpdatedAt = now,
                Remark = "tag for demostration"
            },
            new Tag {
                Id = 2,
                Name = "yaml-front-matter",
                Alias = UrlHelper.ToUrlSafeString("yaml-front-matter"),
                CreatedAt = now,
                UpdatedAt = now,
                Remark = "tag for demostration"
            });

            #region Article

            builder.Entity<Article>().HasData(new Article
            {
                Id = 1,
                Cover = 1,
                Title = "Welcome to ancorazor!",
                Digest = "Learn how to write a post.",
                Alias = UrlHelper.ToUrlSafeString("Welcome to ancorazor!"),
                IsDraft = false,
                Category = 2,
                CreatedAt = now,
                UpdatedAt = now,
                Content = @"---
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

Thanks for reading this guide, hope you can enjoy your writing."
            }, new Article
            {
                Id = 2,
                Cover = 1,
                Title = "欢迎使用ancorazor!",
                Digest = "本文将向你演示如何写一篇文章.",
                Alias = UrlHelper.ToUrlSafeString("Getting start with ancorazor"),
                IsDraft = false,
                Category = 2,
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now,
                Content = @"---
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

感谢阅读本篇教程，祝您写作愉快~"
            });
            builder.Entity<ArticleTags>().HasData(new ArticleTags
            {
                Id = 1,
                Article = 1,
                Tag = 1,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ArticleTags
            {
                Id = 2,
                Article = 1,
                Tag = 2,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ArticleTags
            {
                Id = 3,
                Article = 2,
                Tag = 1,
                CreatedAt = now,
                UpdatedAt = now
            },
            new ArticleTags
            {
                Id = 4,
                Article = 2,
                Tag = 2,
                CreatedAt = now,
                UpdatedAt = now
            });
            builder.Entity<ImageStorage>().HasData(new ImageStorage
            {
                Id = 1,
                CreatedAt = now,
                UpdatedAt = now,
                Size = 0,
                Uploader = 1,
                Category = "cover",
                Path = "upload/default/post-bg.jpg",
                Remark = "default post cover"
            });

            #endregion

            #endregion
        }
    }
}