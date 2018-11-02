using System.ServiceModel;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Contracts
{
    [ServiceContract]
    public interface IPurchaseOrderStatusService : ICommandService<PurchaseOrderStatusDto>
    {
    }
}