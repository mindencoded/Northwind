using System.ServiceModel;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IInventoryTransactionService : IMaintenanceService<InventoryTransactionDto>
    {
    }
}