using System.ServiceModel;
using System.ServiceModel.Web;

namespace S3K.RealTimeOnline.CoreService.Contracts
{
    [ServiceContract]
    public interface ICrudCustomerService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/SelectCustomer",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void SelectCustomer();
    }
}