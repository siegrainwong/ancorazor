using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Entity.Migrations
{
    public partial class AddPostSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "CreatedAt", "UpdatedAt" },
                values: new object[] { @"---
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
5. category：optional, categorize an article and can be used on part of the url depends on your site setting, ancorazor doesn't support *multiple categories*  so use category instead.
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

Thanks for reading this guide, hope you can enjoy your writing.", new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "CreatedAt", "UpdatedAt" },
                values: new object[] { @"---
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
5. category 分类：可选，为文章分类，且可在站点配置中将其作为 Url 的一部分，注意 ancorazor 只支持单分类不支持多分类，所以不要写 categories
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

感谢阅读本篇教程，祝您写作愉快~", new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "ImageStorage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849), new DateTime(2019, 6, 15, 22, 52, 1, 500, DateTimeKind.Local).AddTicks(6849) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "CreatedAt", "UpdatedAt" },
                values: new object[] { @"---
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

Basically same as this [documentation](http://assemble.io/docs/YAML-front-matter.html) except ancorazor doesn't support *multiple categories*.

So, use `category` instead of `categories`.

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

Thanks for reading this guide, hope you can enjoy your writing.", new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Article",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "CreatedAt", "UpdatedAt" },
                values: new object[] { @"---
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
5. category 分类：可选，为文章分类，且可在站点配置中将其作为 Url 的一部分，注意 ancorazor 只支持单分类不支持多分类，所以不要写 categories
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

感谢阅读本篇教程，祝您写作愉快~", new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ArticleTags",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "ImageStorage",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthUpdatedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943), new DateTime(2019, 6, 15, 22, 5, 32, 674, DateTimeKind.Local).AddTicks(5943) });
        }
    }
}
