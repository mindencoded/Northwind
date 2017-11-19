using System.Threading.Tasks;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);

        Task HandleAsync(object command);
    }
}