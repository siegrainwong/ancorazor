#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    /// Article
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Author
        /// </summary>
        public int? Author { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// CommentCount
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Cover
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Digest
        /// </summary>
        public string Digest { get; set; }

        /// <summary>
        /// Alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// IsDraft
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// ViewCount
        /// </summary>
        public int ViewCount { get; set; }
    }
}