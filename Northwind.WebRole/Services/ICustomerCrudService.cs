using System.ServiceModel;
using System.ServiceModel.Web;
using Northwind.Shared;

namespace Northwind.WebRole.Services
{
    [ServiceContract]
    public interface ICustomerCrudService : ICrudService<CustomerDto>
    {
        [OperationContract]
        [WebGet(
            UriTemplate = "/Move",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        void Move();
    }
}