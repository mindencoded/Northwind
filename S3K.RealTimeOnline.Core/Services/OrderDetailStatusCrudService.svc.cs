using System.ServiceModel;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class OrderDetailStatusCrudService :
        CrudService<IBusinessUnitOfWork, OrderDetailStatus, OrderDetailStatusDto>,
        IOrderDetailStatusCrudService
    {
        public OrderDetailStatusCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IOrderDetailStatusCrudService>(config, "");
        }
    }
}