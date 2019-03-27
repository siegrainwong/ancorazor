#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    ///     Tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        ///     Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}