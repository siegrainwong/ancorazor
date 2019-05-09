using Blog.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Entity
{
    public partial class Category: BaseEntity
    {
        public Category()
        {
            ArticleCategories = new HashSet<ArticleCategories>();
        }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        [InverseProperty("CategoryNavigation")]
        public virtual ICollection<ArticleCategories> ArticleCategories { get; set; }
    }
}