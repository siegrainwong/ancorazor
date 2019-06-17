using Ancorazor.Entity.Base;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Ancorazor.Entity
{
    public class SiteSetting: BaseEntity
    {
        /// <summary>
        /// 用于首页大标题跟 NavBar 标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 首页副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 用于 title 节点跟 Meta og:site_name
        /// </summary>
        [Required]
        public string SiteName { get; set; }

        [Required]
        public string Copyright { get; set; }

        /// <summary>
        /// 用于首页的 Meta keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 首页封面图
        /// </summary>
        [Required]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 新文章的模板
        /// </summary>
        public string ArticleTemplate { get; set; }

        /// <summary>
        /// 路由映射
        /// </summary>
        /// <example>
        /// 1. id
        /// 2. alias
        /// 3. date/alias
        /// 4. category/alias
        /// 5. date/category/alias
        /// </example>
        [Required]
        public string RouteMapping { get; set; }

        /// <summary>
        /// Gitment设置
        /// {
        ///     GithubId
        ///     RepositoryName
        ///     ClientId
        ///     ClientSecret
        /// }
        /// </summary>
        [StringLength(500)]
        public string Gitment { get; set; }
    }
}
