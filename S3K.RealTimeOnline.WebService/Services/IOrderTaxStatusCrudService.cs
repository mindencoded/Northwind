using System.ServiceModel;

namespace S3K.RealTimeOnline.WebService.Services
{
    [ServiceContract]
    public interface IOrderTaxStatusCrudService : ICrudService<OrderTaxStatusDto>
    {
    }
}