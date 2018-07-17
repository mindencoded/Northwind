using System.Collections.Generic;
using System.Runtime.Serialization;
using Northwind.WebRole.Tools;

namespace Northwind.WebRole.QueryHandlers
{
    [DataContract]
    public class GenericCountQuery : IQuery<int>
    {
        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }
    }
}