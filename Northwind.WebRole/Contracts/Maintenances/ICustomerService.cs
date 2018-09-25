using System.ServiceModel;
using System.ServiceModel.Web;
using Northwind.WebRole.Dtos;

namespace Northwind.WebRole.Contracts.Maintenances
{
    [ServiceContract]
    public interface ICustomerService : IMaintenanceService<CustomerDto>
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