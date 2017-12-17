using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Core.Decorators
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
            var validationContext = new ValidationContext(command, null, null);
            Validator.ValidateObject(command, validationContext, true);
            _decorated.Handle(command);
        }

        public async Task HandleAsync(TCommand command)
        {
            var validationContext = new ValidationContext(command, null, null);
            Validator.ValidateObject(command, validationContext, true);
            await _decorated.HandleAsync(command);
        }
    }
}