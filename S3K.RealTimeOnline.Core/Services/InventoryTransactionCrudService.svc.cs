using System.ServiceModel;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class InventoryTransactionCrudService :
        CrudService<IBusinessUnitOfWork, InventoryTransaction, InventoryTransactionDto>,
        IInventoryTransactionCrudService
    {
        public InventoryTransactionCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IInventoryTransactionCrudService>(config, "");
        }
    }
}