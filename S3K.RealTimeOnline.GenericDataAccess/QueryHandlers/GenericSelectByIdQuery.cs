using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.GenericDataAccess.QueryHandlers
{
    public class GenericSelectByIdQuery<TEntity> : IQuery<TEntity> where TEntity : Entity
    {
        public object Id { get; set; }
    }
}
