#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    /// ArticleCategories
    /// </summary>
    public class ArticleCategories
    {
        /// <summary>
        /// Article
        /// </summary>
        public int Article { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
    }
}