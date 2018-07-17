using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface ICrudService<TDto> : IService where TDto : class
    {
        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectA(string page, string pageSize);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}/{orderby}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectB(string page, string pageSize, string orderby);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}/{orderby}/{filter}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectC(string page, string pageSize, string orderby, string filter);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}/{orderby}/{filter}/{select}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream SelectD(string page, string pageSize, string orderby, string filter, string select);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/")]
        void Insert(TDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PUT",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/{id}")]
        void Update(string id, TDto dto);

        [OperationContract]
        [WebInvoke(
            Method = "PATCH",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/{id}")]
        void PartialUpdate(string id, string data);

        [OperationContract]
        [WebInvoke(
            Method = "DELETE",
            UriTemplate = "/{id}")]
        void DeleteById(string id);

        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/{id}")]
        Stream SelectById(string id);
    }
}