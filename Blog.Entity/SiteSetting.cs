using Blog.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace Blog.Entity
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
        /// 用于 title 节点跟 meta og:site_name
        /// </summary>
        [Required]
        public string SiteName { get; set; }

        [Required]
        public string Copyright { get; set; }

        /// <summary>
        /// 首页封面图
        /// </summary>
        [Required]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 新文章的模板
        /// </summary>
        public string ArticleTemplate { get; set; }
    }
}
