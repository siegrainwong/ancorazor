using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class Tag: BaseEntity
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTags>();
        }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        [InverseProperty("TagNavigation")]
        public virtual ICollection<ArticleTags> ArticleTags { get; set; }
    }
}