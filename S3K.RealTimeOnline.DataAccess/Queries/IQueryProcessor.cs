namespace S3K.RealTimeOnline.DataAccess.Queries
{
    public interface IQueryProcessor
    {
        //TResult Process<TResult>(IQuery<TResult> query);

        TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}