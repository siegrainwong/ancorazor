using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.EF.Entity.Base
{
    public class Entity<T> where T: struct
    {
        [Key]
        public T Id { get; set; }
    }
}
