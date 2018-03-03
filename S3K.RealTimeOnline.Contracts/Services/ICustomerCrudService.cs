using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
{
    [ServiceContract]
    public interface ICustomerCrudService : ICrudService<CustomerDto>
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/Move",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void Move();
    }
}