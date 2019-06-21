using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class Category: BaseEntity
    {
        public Category()
        {
            Article = new HashSet<Article>();
        }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Alias { get; set; }

        [InverseProperty("CategoryNavigation")]
        public virtual ICollection<Article> Article { get; set; }
    }
}