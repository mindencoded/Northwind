using System.Collections.Generic;
using System.Runtime.Serialization;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    [DataContract]
    public class GenericCountQuery : IQuery<int>
    {
        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }
    }
}