using System.ServiceModel;

namespace S3K.RealTimeOnline.WebService.Services
{
    [ServiceContract]
    public interface IPurchaseOrderCrudService : ICrudService<PurchaseOrderDto>
    {
    }
}