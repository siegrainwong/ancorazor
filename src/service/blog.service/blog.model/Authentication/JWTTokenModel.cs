namespace Blog.Model.Authentication
{
    /// <summary>
    /// 认证实体
    /// </summary>
    public class JWTTokenModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 职能
        /// </summary>
        public string Work { get; set; }
    }
}
