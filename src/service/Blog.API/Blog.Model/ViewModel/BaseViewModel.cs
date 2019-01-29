using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model.ViewModel
{
    public class BaseViewModel
    {
        public int Id { get; set; } = 0;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Remark { get; set; } = "";
    }
}
