#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    ///     UserRole
    /// </summary>
    public class UserRole
    {
        /// <summary>
        ///     Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     RoleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        ///     CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        ///     IsDeleted
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}