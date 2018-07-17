using System.ServiceModel;
using Northwind.Shared.Dtos;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IInventoryTransactionCrudService : ICrudService<InventoryTransactionDto>
    {
    }
}