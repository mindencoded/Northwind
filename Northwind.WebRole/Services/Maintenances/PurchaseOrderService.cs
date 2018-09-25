using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Northwind.WebRole.Contracts.Maintenances;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.Dtos;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services.Maintenances
{
    [RoutePrefix("PurchaseOrders.svc")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PurchaseOrderService : MaintenanceService<IBusinessUnitOfWork, PurchaseOrder, PurchaseOrderDto>,
        IPurchaseOrderService
    {
        public PurchaseOrderService(IUnityContainer container) : base(container)
        {
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Select")]
        public override Stream SelectA(string page, string pageSize, string orderby)
        {
            return base.SelectA(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectB(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectC(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Insert")]
        public override void Insert(PurchaseOrderDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Update")]
        public override void Update(string id, PurchaseOrderDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "PurchaseOrder.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }

        public static void Configure(ServiceConfiguration config)
        {
            Configure<IPurchaseOrderService>(config, "");
        }
    }
}