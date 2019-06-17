using Ancorazor.Entity;
using System.Collections.Generic;
using ArticleModel = Ancorazor.Entity.Article;

namespace Ancorazor.API.Messages.Article
{
    public class ArticleViewModel : ArticleModel
    {
        public new string Author { get; set; }
        public new string Cover { get; set; }
        public string Path { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public ArticleViewModel Previous { get; set; }
        public ArticleViewModel Next { get; set; }
        public string CategoryAlias { get; set; }
    }
}
