using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Northwind.DataTransferObjects;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface IAccountService : IService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/login",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Login(LoginDto login);
    }
}