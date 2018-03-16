using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class PurchaseOrderCrudService : CrudService<IBusinessUnitOfWork, PurchaseOrder, PurchaseOrderDto>, IPurchaseOrderCrudService
    {
        public PurchaseOrderCrudService(IUnityContainer container) : base(container)
        {
        }
    }
}
