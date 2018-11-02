using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Northwind.WebRole.Contracts;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services
{
    [RoutePrefix("CustomerOrders.svc")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerOrderService : QueryService<IBusinessUnitOfWork, CustomerOrder>, ICustomerOrderService
    {
        public CustomerOrderService(IUnityContainer container) : base(container)
        {
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerOrder.Select")]
        public override Stream Select(string page, string pageSize, string orderby, string filter)
        {
            return base.Select(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerOrder.Select")]
        public override Stream Select(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.Select(page, pageSize, orderby, filter, select);
        }

        public static void Configure(ServiceConfiguration config)
        {
            Configure<ICustomerOrderService>(config, "");
        }
    }
}