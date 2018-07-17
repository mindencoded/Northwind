using System.Threading.Tasks;

namespace S3K.RealTimeOnline.WebService.Tools
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);

        Task<TResult> HandleAsync(TQuery query);
    }
}