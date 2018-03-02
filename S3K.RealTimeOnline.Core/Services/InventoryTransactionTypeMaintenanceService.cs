using System.IO;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Core.Services
{
    public partial class MaintenanceService : IInventoryTransactionTypeMaintenanceService
    {
        public Stream SelectInventoryTransactionType(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, InventoryTransactionType>(page, pageSize);
        }

        public void InsertInventoryTransactionType(InventoryTransactionTypeDto dto)
        {
            Insert<IBusinessUnitOfWork, InventoryTransactionType, InventoryTransactionTypeDto>(dto);
        }

        public void UpdateInventoryTransactionType(string id, InventoryTransactionTypeDto dto)
        {
            Update<IBusinessUnitOfWork, InventoryTransactionType, InventoryTransactionTypeDto>(id, dto);
        }

        public void PartialUpdateInventoryTransactionType(string id, string json)
        {
            PartialUpdate<IBusinessUnitOfWork, InventoryTransactionType>(id, json);
        }

        public void DeleteInventoryTransactionTypeById(string id)
        {
            DeleteById<IBusinessUnitOfWork, InventoryTransactionType>(id);
        }

        public Stream SelectInventoryTransactionTypeById(string id)
        {
            return SelectById<IBusinessUnitOfWork, InventoryTransactionType>(id);
        }
    }
}
