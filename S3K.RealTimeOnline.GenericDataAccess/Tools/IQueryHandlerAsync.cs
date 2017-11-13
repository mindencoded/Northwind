using System.Threading.Tasks;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public interface IQueryHandlerAsync<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}