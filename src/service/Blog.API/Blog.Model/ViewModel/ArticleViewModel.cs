#region

using System;
using System.Collections.Generic;
using Blog.Model.Mapping;
using Blog.Model.ParameterModel;
using Blog.Model.ParameterModel.Base;

#endregion

namespace Blog.Model.ViewModel
{
    public class ArticleViewModel : BaseViewModel
    {
        public string Author { get; set; }

        public string Title { get; set; }
        
        public string Digest { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public int ViewCount { get; set; }

        public int CommentCount { get; set; }
    }

    public class ArticlePropertyMapping : PropertyMapping<ArticleViewModel, Article>
    {
        public ArticlePropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>>
                (StringComparer.OrdinalIgnoreCase)
                {
                    [nameof(ArticleViewModel.Title)] = new List<MappedProperty>
                    {
                        new MappedProperty{ Name = nameof(Article.Title), Revert = false}
                    }
                })
        {
        }
    }
}
