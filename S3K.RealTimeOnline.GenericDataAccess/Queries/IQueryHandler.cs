namespace S3K.RealTimeOnline.GenericDataAccess.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}