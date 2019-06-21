using Ancorazor.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ancorazor.Entity
{
    public class OperationLog: BaseEntity
    {
        public int? UserId { get; set; }
        [StringLength(200)]
        public string LoginName { get; set; }
        [StringLength(200)]
        public string Area { get; set; }
        [StringLength(200)]
        public string Controller { get; set; }
        [StringLength(200)]
        public string Action { get; set; }
        [StringLength(50)]
        public string IPAddress { get; set; }
    }
}