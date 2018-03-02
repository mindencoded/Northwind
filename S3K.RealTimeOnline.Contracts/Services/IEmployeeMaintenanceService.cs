using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
{
    [ServiceContract]
    public interface IEmployeeMaintenanceService : IService
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/Select?page={page}&pagesize={pageSize}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectEmployee(string page, string pageSize);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Insert")]
        void InsertEmployee(EmployeeDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/Update?id={id}")]
        void UpdateEmployee(string id, EmployeeDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PATCH",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/PartialUpdate?id={id}")]
        void PartialUpdateEmployee(string id, string data);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/DeleteById?id={id}")]
        void DeleteEmployeeById(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SelectById?id={id}")]
        Stream SelectEmployeeById(string id);
    }
}