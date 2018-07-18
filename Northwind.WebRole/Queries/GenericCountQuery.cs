using System.Collections.Generic;
using System.Runtime.Serialization;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    [DataContract]
    public class GenericCountQuery : IQuery<int>
    {
        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }
    }
}