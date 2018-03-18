using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class OrderTaxStatusCrudService : CrudService<IBusinessUnitOfWork, OrderTaxStatus, OrderTaxStatusDto>,
        IOrderTaxStatusCrudService
    {
        public OrderTaxStatusCrudService(IUnityContainer container) : base(container)
        {
        }
    }
}