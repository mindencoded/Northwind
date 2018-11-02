using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Northwind.WebRole.Contracts
{
    [ServiceContract]
    public interface IQueryService : IService
    {
        [OperationContract(Name = "FirstSelect")]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}/{orderby}/{*filter}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Select(string page, string pageSize, string orderby, string filter);

        [OperationContract(Name = "SecondSelect")]
        [WebInvoke(
            Method = "GET",
            UriTemplate = "/{page}/{pageSize}/{orderby}/{filter}/{*select}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Select(string page, string pageSize, string orderby, string filter, string select);
    }
}
