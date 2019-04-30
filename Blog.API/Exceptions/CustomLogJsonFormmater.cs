using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Exceptions
{
    public class CustomLogJsonFormmater: ElasticsearchJsonFormatter
    {
        protected override void WriteRenderedMessage(string message, ref string delim, TextWriter output)
        {

            base.WriteRenderedMessage(message, ref delim, output);
        }
    }
}
