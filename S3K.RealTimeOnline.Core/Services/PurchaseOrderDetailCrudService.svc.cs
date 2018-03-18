using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class PurchaseOrderDetailCrudService :
        CrudService<IBusinessUnitOfWork, PurchaseOrderDetail, PurchaseOrderDetailDto>, IPurchaseOrderDetailCrudService
    {
        public PurchaseOrderDetailCrudService(IUnityContainer container) : base(container)
        {
        }
    }
}