using System.ComponentModel.DataAnnotations;
using S3K.RealTimeOnline.GenericDataAccess.Commands;

namespace S3K.RealTimeOnline.GenericDataAccess.Decorators
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
    }
}