namespace S3K.RealTimeOnline.GenericDataAccess.Tools
{
    public interface IQueryProcessor
    {
        TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}