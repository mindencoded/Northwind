﻿using System.IO;
using System.Security.Permissions;
using System.ServiceModel;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.Dtos;
using Northwind.WebRole.UnitOfWork;
using Unity;

namespace Northwind.WebRole.Services
{
    public class InvoiceCrudService : CrudService<IBusinessUnitOfWork, Invoice, InvoiceDto>, IInvoiceCrudService
    {
        public InvoiceCrudService(IUnityContainer container) : base(container)
        {
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Select")]
        public override Stream SelectA(string page, string pageSize, string orderby)
        {
            return base.SelectA(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectB(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectC(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Insert")]
        public override void Insert(InvoiceDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Update")]
        public override void Update(string id, InvoiceDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "InvoiceCrud.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }

        public static void Configure(ServiceConfiguration config)
        {
            Configure<IInvoiceCrudService>(config, "");
        }
    }
}