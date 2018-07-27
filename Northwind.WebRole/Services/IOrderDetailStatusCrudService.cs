using System.ServiceModel;
using Northwind.DataTransferObjects;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IOrderDetailStatusCrudService : ICrudService<OrderDetailStatusDto>
    {
    }
}