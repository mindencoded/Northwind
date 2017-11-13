using System.Collections.Generic;
using System.Linq;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class Query<TResult> : IQuery<TResult>
    {
        public int OffSet { get; set; }

        public int Limit { get; set; }

        public string OrderBy { get; set; }

        public void BuildOrderBy(Dictionary<string, SortDirection> entries)
        {
            IList<string> orderBy = new List<string>();
            foreach (KeyValuePair<string, SortDirection> entry in entries)
            {
                orderBy.Add(entry.Key + " " + entry.Value.ToString().ToUpper());
            }
            OrderBy = string.Join(",", orderBy.ToArray());
        }
    }
}