using System.Threading.Tasks;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Decorators
{
    public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            ValidationHelper.ValidateObject(command);
            _decorated.Handle(command);
        }

        public async Task HandleAsync(TCommand command)
        {
            ValidationHelper.ValidateObject(command);
            await _decorated.HandleAsync(command);
        }
    }
}