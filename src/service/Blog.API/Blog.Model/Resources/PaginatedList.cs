#region

using System.Collections.Generic;

#endregion

namespace Blog.Model.Resources
{
    public class PaginatedList<T> : List<T> where T : class
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        private int _totalItemsCount;
        public int TotalItemsCount
        {
            get => _totalItemsCount;
            set => _totalItemsCount = value >= 0 ? value : 0;
        }

        public int PageCount => TotalItemsCount / PageSize + (TotalItemsCount % PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageIndex > 0;
        public bool HasNext => PageIndex < PageCount - 1;

        public PaginatedList(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemsCount = totalItemsCount;
            AddRange(data);
        }
    }
}
