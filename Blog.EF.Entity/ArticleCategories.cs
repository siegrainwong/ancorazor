using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class ArticleCategories
    {
        public int Id { get; set; }
        public int Article { get; set; }
        public int Category { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Article")]
        [InverseProperty("ArticleCategories")]
        public virtual Article ArticleNavigation { get; set; }
        [ForeignKey("Category")]
        [InverseProperty("ArticleCategories")]
        public virtual Category CategoryNavigation { get; set; }
    }
}