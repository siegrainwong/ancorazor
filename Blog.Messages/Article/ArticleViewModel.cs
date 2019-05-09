using ArticleModel = Blog.Entity.Article;

namespace Blog.API.Messages.Article
{
    public class ArticleViewModel : ArticleModel
    {
        public string Path { get; set; }
    }
}
