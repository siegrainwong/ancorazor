using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class ArticleTags
    {
        public int Id { get; set; }
        public int Article { get; set; }
        public int Tag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Article")]
        [InverseProperty("ArticleTags")]
        public virtual Article ArticleNavigation { get; set; }
        [ForeignKey("Tag")]
        [InverseProperty("ArticleTags")]
        public virtual Tag TagNavigation { get; set; }
    }
}