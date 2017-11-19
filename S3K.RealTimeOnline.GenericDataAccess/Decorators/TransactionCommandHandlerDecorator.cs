using System.Threading.Tasks;
using System.Transactions;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            using (var scope = new TransactionScope())
            {
                _decorated.Handle(command);
                scope.Complete();
            }
        }

        public Task HandleAsync(object command)
        {
            throw new System.NotImplementedException();
        }
    }
}