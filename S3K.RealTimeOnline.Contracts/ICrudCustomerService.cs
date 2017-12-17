using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts
{
    [ServiceContract]
    public interface ICrudCustomerService : IMainService
    {
        [OperationContract] 
        [WebGet(
            UriTemplate = "/SelectCustomer",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectCustomer();

        [OperationContract]
        [WebInvoke(
            Method = "POST", 
            BodyStyle = WebMessageBodyStyle.Bare, 
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json, 
            UriTemplate = "/InsertCustomer")]
        void InsertCustomer(CustomerDto customer);

        [OperationContract]
        [WebGet(
            UriTemplate = "/MoveCustomer",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void MoveCustomer();
    }
}