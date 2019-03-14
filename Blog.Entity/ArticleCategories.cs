#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    ///     ArticleCategories
    /// </summary>
    public class ArticleCategories
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Article
        /// </summary>
        public int Article { get; set; }

        /// <summary>
        ///     Category
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        ///     CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}