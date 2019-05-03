using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class Users
    {
        public Users()
        {
            Article = new HashSet<Article>();
            UserRole = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string LoginName { get; set; }
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
        [StringLength(60)]
        public string RealName { get; set; }
        public int Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AuthUpdatedAt { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("AuthorNavigation")]
        public virtual ICollection<Article> Article { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}