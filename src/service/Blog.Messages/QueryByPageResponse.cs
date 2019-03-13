namespace Blog.API.Messages
{
    public class QueryByPageResponse<TItem> : QueryResponse<TItem>
    {
        private int _total;
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;

        public int Total
        {
            get => _total;
            set => _total = value >= 0 ? value : 0;
        }

        public int PageCount => Total <= 0 ? 0 : Total / PageSize + (Total % PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => PageIndex < PageCount - 1;
    }
}