using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Dtos;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class ProductCrudService : CrudService<IBusinessUnitOfWork, Product, ProductDto>, IProductCrudService
    {
        public ProductCrudService(IUnityContainer container) : base(container)
        {
        }
    }
}