using Blog.EF.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class Tag: BaseEntity<int>
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