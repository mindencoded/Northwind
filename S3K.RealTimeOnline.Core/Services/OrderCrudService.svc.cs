using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class OrderCrudService : CrudService<IBusinessUnitOfWork, Order, OrderDto>, IOrderCrudService
    {
        public OrderCrudService(IUnityContainer container) : base(container)
        {
        }
    }
}