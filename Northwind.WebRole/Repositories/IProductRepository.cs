using Northwind.WebRole.Domain.Business;

namespace Northwind.WebRole.Repositories
{
    public interface IProductRepository : ICommandRepository<Product>
    {
    }
}