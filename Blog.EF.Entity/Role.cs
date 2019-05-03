using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}