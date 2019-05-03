using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class Article
    {
        public Article()
        {
            ArticleCategories = new HashSet<ArticleCategories>();
            ArticleTags = new HashSet<ArticleTags>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Cover { get; set; }
        public int? Author { get; set; }
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
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        public bool IsDraft { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        [ForeignKey("Author")]
        [InverseProperty("Article")]
        public virtual Users AuthorNavigation { get; set; }
        [InverseProperty("ArticleNavigation")]
        public virtual ICollection<ArticleCategories> ArticleCategories { get; set; }
        [InverseProperty("ArticleNavigation")]
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
    }
}