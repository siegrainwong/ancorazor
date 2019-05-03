using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class Tag
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTags>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        [InverseProperty("TagNavigation")]
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
    }
}