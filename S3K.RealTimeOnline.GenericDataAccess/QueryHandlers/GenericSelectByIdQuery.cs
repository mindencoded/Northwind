using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    public class GenericSelectByIdQuery : IQuery<Entity>
    {
        public object Id { get; set; }
    }
}