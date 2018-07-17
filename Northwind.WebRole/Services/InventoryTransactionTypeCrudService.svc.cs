using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using Northwind.Shared.Dtos;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;
using Unity;

namespace Northwind.WebRole.Services
{
    public class InventoryTransactionTypeCrudService :
        CrudService<IBusinessUnitOfWork, InventoryTransactionType, InventoryTransactionTypeDto>,
        IInventoryTransactionTypeCrudService
    {
        public InventoryTransactionTypeCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IInventoryTransactionTypeCrudService>(config, "");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Select")]
        public override Stream SelectA(string page, string pageSize)
        {
            return base.SelectA(page, pageSize);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby)
        {
            return base.SelectB(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectC(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Select")]
        public override Stream SelectD(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectD(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Insert")]
        public override void Insert(InventoryTransactionTypeDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Update")]
        public override void Update(string id, InventoryTransactionTypeDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InventoryTransactionTypeCrud.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }
    }
}