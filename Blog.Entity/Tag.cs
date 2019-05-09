using Blog.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Entity
{
    public partial class Tag: BaseEntity
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTags>();
        }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [InverseProperty("TagNavigation")]
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
    }
}