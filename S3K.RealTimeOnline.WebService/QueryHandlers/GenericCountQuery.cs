using System.Collections.Generic;
using System.Runtime.Serialization;
using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    [DataContract]
    public class GenericCountQuery : IQuery<int>
    {
        [DataMember] public IList<ParameterBuilder> Conditions { get; set; }
    }
}