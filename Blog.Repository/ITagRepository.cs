#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    public interface ITagRepository : IRepositoryAsync<Tag, int>
    {
        Task SetArticleTagsAsync(int articleId, string[] tags);
    }
}

//---
//title: 这是标题
//date: 2018-12-03 00:00
//categories:
//- 分类1
//- 分类2
//tags:
//- 标签1
//- 标签2
//---

//这是内容