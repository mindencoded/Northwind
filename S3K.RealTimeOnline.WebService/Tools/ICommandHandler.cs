using System.Threading.Tasks;

namespace S3K.RealTimeOnline.WebService.Tools
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);

        Task HandleAsync(TCommand command);
    }
}