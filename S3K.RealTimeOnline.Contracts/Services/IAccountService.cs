using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.Dtos;

namespace S3K.RealTimeOnline.Contracts.Services
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