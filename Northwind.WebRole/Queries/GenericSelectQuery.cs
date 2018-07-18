using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    [DataContract]
    public class GenericSelectQuery : IQuery<IEnumerable<ExpandoObject>>
    {
        [DataMember] public IList<string> Columns { get; set; }

        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }

        [DataMember] public string OrderBy { get; set; }

        [DataMember] public int? Page { get; set; }

        [DataMember] public int? PageSize { get; set; }
    }
}