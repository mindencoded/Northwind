using System.ServiceModel;
using System.ServiceModel.Web;

namespace S3K.RealTimeOnline.CoreService.Contracts
{
    [ServiceContract]
    public interface ICrudEmployeeService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/SelectEmployee",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void SelectEmployee();
    }
}