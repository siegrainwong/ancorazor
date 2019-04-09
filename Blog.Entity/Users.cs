#region

using System;

#endregion

namespace Blog.Entity
{
    /// <summary>
    /// Users
    /// </summary>
    public class Users
    {
        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// LoginName
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RealName
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 上次凭据变更时间
        /// 
        /// 当该时间大于凭据 Cookie 中的 AuthUpdatedAt 值时，需要清除 Cookie 让用户重新登录
        /// </summary>
        public DateTime AuthUpdatedAt { get; set; }
    }
}