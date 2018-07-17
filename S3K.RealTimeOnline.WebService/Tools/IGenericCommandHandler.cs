using System.Threading.Tasks;

namespace S3K.RealTimeOnline.WebService.Tools
{
    public interface IGenericCommandHandler
    {
        void Handle<TEntity>(object command) where TEntity : class;

        Task HandleAsync<TEntity>(object command) where TEntity : class;
    }
}