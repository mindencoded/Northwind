using System.ServiceModel;
using Northwind.Shared.Dtos;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IProductCrudService : ICrudService<ProductDto>
    {
    }
}