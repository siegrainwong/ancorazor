using Blog.Entity;
using System.Collections.Generic;
using ArticleModel = Blog.Entity.Article;

namespace Blog.API.Messages.Article
{
    public class ArticleViewModel : ArticleModel
    {
        public new string Author { get; set; }
        public string Path { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public ArticleViewModel Previous { get; set; }
        public ArticleViewModel Next { get; set; }
    }
}
