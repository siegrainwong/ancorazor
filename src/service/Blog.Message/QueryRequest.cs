#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Blog.Message
{
    public class QueryRequest
    {
        [Range(1, 100)] public int Taken { get; set; }
    }
}