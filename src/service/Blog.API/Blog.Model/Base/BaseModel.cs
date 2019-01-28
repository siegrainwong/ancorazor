using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model.Base
{
    public class BaseModel
    {
        public BaseModel()
        {

        }
        public int Id { get; set; } = 0;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Remark { get; set; } = "";
        public bool? IsDeleted { get; set; }
    }
}
