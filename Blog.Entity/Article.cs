using Blog.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Entity
{
    public partial class Article: BaseEntity
    {
        public Article()
        {
            ArticleCategories = new HashSet<ArticleCategories>();
            ArticleTags = new HashSet<ArticleTags>();
        }
        [Required]
        public int Cover { get; set; }
        public int Author { get; set; }
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
        [StringLength(500)]
        public string Digest { get; set; }
        [Required]
        [StringLength(256)]
        public string Alias { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsDraft { get; set; }

        [ForeignKey("Cover")]
        [InverseProperty("Article")]
        public virtual ImageStorage ImageStorageNavigation { get; set; }
        [ForeignKey("Author")]
        [InverseProperty("Article")]
        public virtual Users AuthorNavigation { get; set; }
        [InverseProperty("ArticleNavigation")]
        public virtual ICollection<ArticleCategories> ArticleCategories { get; set; }
        [InverseProperty("ArticleNavigation")]
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
    }
}