using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
{
    [ServiceContract]
    public interface IInventoryTransactionMaintenanceService : IService
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/Select?page={page}&pagesize={pageSize}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectInventoryTransaction(string page, string pageSize);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Insert")]
        void InsertInventoryTransaction(InventoryTransactionDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Update?id={id}")]
        void UpdateInventoryTransaction(string id, InventoryTransactionDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PATCH",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/PartialUpdate?id={id}")]
        void PartialUpdateInventoryTransaction(string id, string data);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/DeleteById?id={id}")]
        void DeleteInventoryTransactionById(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SelectById?id={id}")]
        Stream SelectInventoryTransactionById(string id);
    }
}
