using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using Northwind.DataTransferObjects;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;
using Unity;

namespace Northwind.WebRole.Services
{
    public class OrderDetailStatusCrudService :
        CrudService<IBusinessUnitOfWork, OrderDetailStatus, OrderDetailStatusDto>,
        IOrderDetailStatusCrudService
    {
        public OrderDetailStatusCrudService(IUnityContainer container) : base(container)
        {
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Select")]
        public override Stream SelectA(string page, string pageSize)
        {
            return base.SelectA(page, pageSize);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby)
        {
            return base.SelectB(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectC(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Select")]
        public override Stream SelectD(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectD(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Insert")]
        public override void Insert(OrderDetailStatusDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Update")]
        public override void Update(string id, OrderDetailStatusDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "OrderDetailStatusCrud.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IOrderDetailStatusCrudService>(config, "");
        }
    }
}