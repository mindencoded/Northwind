using System.IO;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Core.Services
{
    public partial class MaintenanceService : IInventoryTransactionMaintenanceService
    {
        public Stream SelectInventoryTransaction(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, InventoryTransaction>(page, pageSize);
        }

        public void InsertInventoryTransaction(InventoryTransactionDto dto)
        {
            Insert<IBusinessUnitOfWork, InventoryTransaction, InventoryTransactionDto>(dto);
        }

        public void UpdateInventoryTransaction(string id, InventoryTransactionDto dto)
        {
            Update<IBusinessUnitOfWork, InventoryTransaction, InventoryTransactionDto>(id, dto);
        }

        public void PartialUpdateInventoryTransaction(string id, string json)
        {
            PartialUpdate<IBusinessUnitOfWork, InventoryTransaction>(id, json);
        }

        public void DeleteInventoryTransactionById(string id)
        {
            DeleteById<IBusinessUnitOfWork, InventoryTransaction>(id);
        }

        public Stream SelectInventoryTransactionById(string id)
        {
            return SelectById<IBusinessUnitOfWork, InventoryTransaction>(id);
        }
    }
}
