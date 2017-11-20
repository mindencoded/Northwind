using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.Decorators
{
    public class DeadlockRetryCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _decorated;

        public DeadlockRetryCommandHandlerDecorator(ICommandHandler<TCommand> decorated)
        {
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            HandleWithCountDown(command, 5);
        }

        public async Task HandleAsync(TCommand command)
        {
            await HandleWithCountDownAsync(command, 5);
        }

        private void HandleWithCountDown(TCommand command, int count)
        {
            try
            {
                _decorated.Handle(command);
            }
            catch (Exception ex)
            {
                if (count <= 0 || !IsDeadlockException(ex))
                    throw;

                Thread.Sleep(300);

                HandleWithCountDown(command, count - 1);
            }
        }

        private async Task HandleWithCountDownAsync(TCommand command, int count)
        {
            try
            {
                await _decorated.HandleAsync(command);
            }
            catch (Exception ex)
            {
                if (count <= 0 || !IsDeadlockException(ex))
                    throw;

                await Task.Delay(300);

                HandleWithCountDown(command, count - 1);
            }
        }

        private static bool IsDeadlockException(Exception ex)
        {
            while (ex != null)
            {
                if (ex is DbException && ex.Message.Contains("deadlock"))
                    return true;

                ex = ex.InnerException;
            }

            return false;
        }
    }
}