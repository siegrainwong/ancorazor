using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.EF.Entity.Base
{
    public class BaseEntity<T> : Entity<T> where T: struct
    {
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [StringLength(256)]
        public string Remark { get; set; }
    }
}
