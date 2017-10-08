namespace S3K.RealTimeOnline.DataAccess.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}