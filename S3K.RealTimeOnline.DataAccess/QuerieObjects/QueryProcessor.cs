using S3K.RealTimeOnline.Commons;

namespace S3K.RealTimeOnline.DataAccess.QuerieObjects
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IContainer _container;

        public QueryProcessor(IContainer container)
        {
            _container = container;
        }

        //[DebuggerStepThrough]
        //public TResult Process<TResult>(IQuery<TResult> query)
        //{
        //    var handlerType = typeof(IQueryHandler<,>)
        //        .MakeGenericType(query.GetType(), typeof(TResult));

        //    dynamic handler = container.GetInstance(handlerType);

        //    return handler.Handle((dynamic)query);
        //}

        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _container.Resolve(handlerType);

            return handler.Handle((dynamic)query);
        }
    }
}
