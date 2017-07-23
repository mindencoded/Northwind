namespace S3K.RealTimeOnline.DataAccess.QuerieObjects
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}