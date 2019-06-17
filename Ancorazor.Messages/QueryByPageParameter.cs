#region

#endregion

using System.ComponentModel.DataAnnotations;

namespace Ancorazor.API.Messages
{
    public class PaginationParameter : QueryParameter
    {
        [Range(0, int.MaxValue)] public int PageIndex { get; set; }

        [Range(1, 100)] public int PageSize { get; set; } = 10;
    }

    public class ArticlePaginationParameter: PaginationParameter
    {
        public bool? IsDraft { get; set; } = false;
    }
}