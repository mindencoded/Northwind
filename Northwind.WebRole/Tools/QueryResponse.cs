using System.Collections.Generic;
using System.Dynamic;

namespace Northwind.WebRole.Tools
{
    public class QueryResponse
    {
        public IEnumerable<ExpandoObject> Value { get; set; }

        public int Total { get; set; }

        public int Count { get; set; }
    }
}