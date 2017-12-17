using System.ServiceModel;
using System.ServiceModel.Web;

namespace S3K.RealTimeOnline.Contracts
{
    [ServiceContract]
    public interface ICrudEmployeeService : IMainService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/SelectEmployee",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void SelectEmployee();
    }
}