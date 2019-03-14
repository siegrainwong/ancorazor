#region

using System.Collections.Generic;

#endregion

namespace Blog.Message
{
    public class QueryResponse<TItem>
    {
        public IEnumerable<TItem> List { get; set; }
    }
}