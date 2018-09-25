using System.ServiceModel;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Contracts.Maintenances
{
    [ServiceContract]
    public interface IPurchaseOrderDetailService : IMaintenanceService<PurchaseOrderDetailDto>
    {
    }
}