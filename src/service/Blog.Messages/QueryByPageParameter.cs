#region

#endregion

using System.ComponentModel.DataAnnotations;

namespace Blog.API.Messages
{
    public class QueryByPageParameter : QueryParameter
    {
        [Range(0, int.MaxValue)] public int PageIndex { get; set; }

        [Range(1, 100)] public int PageSize { get; set; } = 10;
    }
}