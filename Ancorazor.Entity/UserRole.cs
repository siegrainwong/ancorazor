using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class UserRole: BaseEntity
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