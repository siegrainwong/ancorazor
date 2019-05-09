using Blog.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Entity
{
    public partial class Role: BaseEntity
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        [StringLength(50)]
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}