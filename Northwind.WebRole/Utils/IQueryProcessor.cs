namespace Northwind.WebRole.Utils
{
    public interface IQueryProcessor
    {
        TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}