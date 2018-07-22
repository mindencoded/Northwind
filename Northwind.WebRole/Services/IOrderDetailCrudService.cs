using System.ServiceModel;
using Northwind.Shared;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IOrderDetailCrudService : ICrudService<OrderDetailDto>
    {
    }
}