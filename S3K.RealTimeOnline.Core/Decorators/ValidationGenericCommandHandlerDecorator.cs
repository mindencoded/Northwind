using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Core.Decorators
{
    public class ValidationGenericCommandHandlerDecorator : IGenericCommandHandler
    {
        private readonly IGenericCommandHandler _decorated;

        public ValidationGenericCommandHandlerDecorator(IGenericCommandHandler decorated)
        {
            _decorated = decorated;
        }

        public void Handle<TEntity>(object command) where TEntity : class
        {
            Validation<TEntity>(command);
            _decorated.Handle<TEntity>(command);
        }

        public async Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            Validation<TEntity>(command);
            await _decorated.HandleAsync<TEntity>(command);
        }

        private void Validation<TEntity>(object command) where TEntity : class
        {
            if (command is TEntity)
            {
                ValidationHelper.ValidateObject(command);
            }
            else if (command is ExpandoObject)
            {
                ValidationHelper.ValidateProperties<TEntity>((ExpandoObject) command);
            }
            else if (command.GetType().IsGenericType &&
                     command.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                ValidationHelper.ValidateProperties<TEntity>((IDictionary<string, object>) command);
            }
            else
            {
                ValidationHelper.ValidateProperties<TEntity>(command);
            }
        }
    }
}