#region

using System.Collections.Generic;

#endregion

namespace Ancorazor.API.Messages
{
    public class QueryResponse<TItem>
    {
        public IEnumerable<TItem> List { get; set; }
    }
}