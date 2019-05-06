using Blog.EF.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class ArticleCategories : BaseEntity
    {
        public int Article { get; set; }
        public int Category { get; set; }

        [ForeignKey("Article")]
        [InverseProperty("ArticleCategories")]
        public virtual Article ArticleNavigation { get; set; }
        [ForeignKey("Category")]
        [InverseProperty("ArticleCategories")]
        public virtual Category CategoryNavigation { get; set; }
    }
}