#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    /// OperationLog
    /// </summary>
    public class OperationLog
    {
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// IPAddress
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// LoginName
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// LogTime
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
    }
}