using System.ServiceModel;

namespace Northwind.WebRole.Contracts
{
    [ServiceContract]
    public interface ICustomerOrderService : IQueryService
    {
    }
}
