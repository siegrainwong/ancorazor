using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class ArticleTags: BaseEntity
    {
        public int Article { get; set; }
        public int Tag { get; set; }

        [ForeignKey("Article")]
        [InverseProperty("ArticleTags")]
        public virtual Article ArticleNavigation { get; set; }
        [ForeignKey("Tag")]
        [InverseProperty("ArticleTags")]
        public virtual Tag TagNavigation { get; set; }
    }
}