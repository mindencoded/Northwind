using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
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
            IList<ValidationResult> validationResults;
            bool isValid = command is TEntity
                ? ValidationHelper.ValidateObject(command, out validationResults)
                : command is ExpandoObject
                    ? ValidationHelper.ValidateProperties<TEntity>((ExpandoObject) command, out validationResults)
                    : command.GetType().IsGenericType &&
                      command.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>)
                        ? ValidationHelper.ValidateProperties<TEntity>((IDictionary<string, object>) command,
                            out validationResults)
                        : ValidationHelper.ValidateProperties<TEntity>(command,
                            out validationResults);

            if (!isValid)
            {
                throw new ValidationException(validationResults.Select(x => x.ErrorMessage).Aggregate((i, j) => i + "," + j));
            }

            _decorated.Handle<TEntity>(command);
        }

        public Task HandleAsync<TEntity>(object command) where TEntity : class
        {
            throw new System.NotImplementedException();
        }
    }
}