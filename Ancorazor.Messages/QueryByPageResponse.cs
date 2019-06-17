namespace Ancorazor.API.Messages
{
    public class PaginationResponse<TItem> : QueryResponse<TItem>
    {
        public bool HasNext => PageIndex < PageCount - 1;
        public int NextPageIndex => HasNext ? PageIndex + 1 : PageIndex;
        public bool HasPrevious => PageIndex > 0;
        public int PreviousPageIndex => HasPrevious ? PageIndex - 1 : PageIndex;
        public int PageCount => Total <= 0 ? 0 : Total / PageSize + (Total % PageSize > 0 ? 1 : 0);
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;

        private int _total;
        public int Total
        {
            get => _total;
            set => _total = value >= 0 ? value : 0;
        }
    }
}