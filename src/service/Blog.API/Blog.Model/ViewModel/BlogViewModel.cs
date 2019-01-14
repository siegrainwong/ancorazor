using System;

namespace Blog.Model.ViewModel
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }
        
        public string Digest { get; set; }

        /// <summary>
        /// 上一篇
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// 上一篇id
        /// </summary>
        public int PreviousId { get; set; }

        /// <summary>
        /// 下一篇
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// 下一篇id
        /// </summary>
        public int NextId { get; set; }

        public string Category { get; set; }

        public string Content { get; set; }

        public int Traffic { get; set; }

        public int CommentCount { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }
    }
}
