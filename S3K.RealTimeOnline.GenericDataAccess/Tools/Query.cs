using System.Runtime.Serialization;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public class Query<TResult> : IQuery<TResult>
    {
        public Query()
        {
            Page = 1;
            PageSize = 200;
        }

        [DataMember]
        public int? Page { get; set; }

        [DataMember]
        public int? PageSize { get; set; }
    }
}