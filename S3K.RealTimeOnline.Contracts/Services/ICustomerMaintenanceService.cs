using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
{
    [ServiceContract]
    public interface ICustomerMaintenanceService : IService
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/Select?page={page}&pagesize={pageSize}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectCustomer(string page, string pageSize);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Insert")]
        void InsertCustomer(CustomerDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Update?id={id}")]
        void UpdateCustomer(string id, CustomerDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PATCH",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/PartialUpdate?id={id}")]
        void PartialUpdateCustomer(string id, string json);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/DeleteById?id={id}")]
        void DeleteCustomerById(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SelectById?id={id}")]
        Stream SelectCustomerById(string id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/Move",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void MoveCustomer();
    }
}