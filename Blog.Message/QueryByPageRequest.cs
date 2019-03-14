#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Blog.Message
{
    public class QueryByPageRequest
    {
        [Range(0, int.MaxValue)] public int PageIndex { get; set; }

        [Range(1, 100)] public int PageSize { get; set; } = 10;

        public bool IsDeleted { get; set; } = false;
    }
}