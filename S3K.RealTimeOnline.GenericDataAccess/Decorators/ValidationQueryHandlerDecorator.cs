using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.GenericDataAccess.Decorators
{
    public class ValidationQueryHandlerDecorator<TQuery, TResult>
        : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated)
        {
            _decorated = decorated;
        }

        public TResult Handle(TQuery query)
        {
            var validationContext = new ValidationContext(query, null, null);
            Validator.ValidateObject(query, validationContext, true);
            return _decorated.Handle(query);
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            var validationContext = new ValidationContext(query, null, null);
            Validator.ValidateObject(query, validationContext, true);
            return await _decorated.HandleAsync(query);
        }
    }
}