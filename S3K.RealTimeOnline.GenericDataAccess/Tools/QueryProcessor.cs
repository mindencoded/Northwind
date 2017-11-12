using Unity;

namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IUnityContainer _container;

        public QueryProcessor(IUnityContainer container)
        {
            _container = container;
        }

        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _container.Resolve(handlerType);

            return handler.Handle((dynamic) query);
        }
    }
}