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
            UriTemplate = "/InsertCustomer")]
        void InsertCustomer(CustomerDto customer);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/UpdateCustomer/{id}")]
        void UpdateCustomer(string id, CustomerDto customer);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/DeleteCustomerById/{id}")]
        void DeleteCustomerById(string id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/MoveCustomer",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void MoveCustomer();
    }
}