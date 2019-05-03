using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.EF.Entity
{
    public partial class OperationLog
    {
        [Key]
        public Guid Guid { get; set; }
        public int? UserId { get; set; }
        [StringLength(200)]
        public string LoginName { get; set; }
        [StringLength(200)]
        public string Area { get; set; }
        [StringLength(200)]
        public string Controller { get; set; }
        [StringLength(200)]
        public string Action { get; set; }
        [Column("IPAddress")]
        [StringLength(50)]
        public string Ipaddress { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
    }
}