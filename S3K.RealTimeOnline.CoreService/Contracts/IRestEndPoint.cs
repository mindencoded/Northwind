using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.WebRole;

namespace S3K.RealTimeOnline.CoreService.Contracts
{
    [ServiceContract]
    public interface IRestEndPoint
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetPlayersXml",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Bare)]
        List<Players> GetPlayersXml();

        [OperationContract]
        [WebGet(UriTemplate = "/GetPlayersJson",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        List<Players> GetPlayersJson();
    }
}