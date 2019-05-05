namespace Blog.API.Common.Constants
{
    public static class Constants
    {
        public static class Article
        {
            public const string DefaultCategoryName = "Uncategorized";
            public const string RouteReplaceRegex = @"([^\w\d])+";
        }

        public static class ErrorCode
        {
            public const string Default = "0001";

            public const string DuplicateEntity = "0010";
        }
    }
}
