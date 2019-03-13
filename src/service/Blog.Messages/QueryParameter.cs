#region

#endregion

using System.ComponentModel.DataAnnotations;

namespace Blog.API.Messages
{
    public class QueryParameter
    {
        public bool IsDeleted { get; set; } = false;
    }
}