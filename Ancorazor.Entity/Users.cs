using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class Users: BaseEntity
    {
        public Users()
        {
            Article = new HashSet<Article>();
            UserRole = new HashSet<UserRole>();
            ImageStorage = new HashSet<ImageStorage>();
        }

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
        public DateTime AuthUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [InverseProperty("AuthorNavigation")]
        public virtual ICollection<Article> Article { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserRole> UserRole { get; set; }
        [InverseProperty("UploaderNavigation")]
        public virtual ICollection<ImageStorage> ImageStorage { get; set; }
    }
}