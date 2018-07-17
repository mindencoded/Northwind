using S3K.RealTimeOnline.WebService.Tools;

namespace S3K.RealTimeOnline.WebService.QueryHandlers
{
    public class GenericSelectByIdQuery : IQuery<Entity>
    {
        public object Id { get; set; }
    }
}