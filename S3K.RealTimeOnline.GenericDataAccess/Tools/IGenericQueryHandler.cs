using System.Threading.Tasks;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public interface IGenericQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle<TEntity>(TQuery query) where TEntity : class;

        Task<TResult> HandleAsync<TEntity>(TQuery query) where TEntity : class;
    }
}