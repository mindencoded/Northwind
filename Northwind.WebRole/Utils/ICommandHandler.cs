using System.Threading.Tasks;

namespace Northwind.WebRole.Utils
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);

        Task HandleAsync(TCommand command);
    }
}