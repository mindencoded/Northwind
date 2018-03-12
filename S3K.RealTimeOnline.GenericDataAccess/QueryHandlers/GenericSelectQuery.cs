using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    [DataContract]
    public class GenericSelectQuery : Query<IEnumerable<ExpandoObject>>
    {
        [DataMember] public IList<string> Columns { get; set; }

        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }

        [DataMember] public string OrderBy { get; set; }
    }
}