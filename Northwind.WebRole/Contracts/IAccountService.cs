using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Contracts
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