using System.ServiceModel;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
{
    [ServiceContract]
    public interface IPurchaseOrderDetailCrudService : ICrudService<PurchaseOrderDetailDto>
    {
    }
}