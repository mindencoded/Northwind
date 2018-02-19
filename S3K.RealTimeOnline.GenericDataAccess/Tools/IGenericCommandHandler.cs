using System.Threading.Tasks;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public interface IGenericCommandHandler
    {
        void Handle<TEntity>(object command) where TEntity : class;

        Task HandleAsync<TEntity>(object command) where TEntity : class;
    }
}