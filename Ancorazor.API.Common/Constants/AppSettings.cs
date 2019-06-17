namespace Ancorazor.API.Common.Constants
{
    /// <summary>
    /// AppSettings.SEOConfiguration
    /// </summary>
    ///
    /// MARK: 配置封装
    /// ref: https://andrewlock.net/how-to-use-the-ioptions-pattern-for-configuration-in-asp-net-core-rc2/
    ///
    public sealed class SEOConfiguration
    {
        public string ArticleRouteMapping { get; set; }
    }

    public sealed class DbConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
