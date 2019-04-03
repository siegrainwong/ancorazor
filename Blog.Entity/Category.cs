#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    /// Category
    /// </summary>
    public class Category
    {
        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}