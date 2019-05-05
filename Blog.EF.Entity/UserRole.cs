using Blog.EF.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class UserRole: BaseEntity<int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UserRole")]
        public virtual Role Role { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("UserRole")]
        public virtual Users User { get; set; }
    }
}