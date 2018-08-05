using System;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Web;
using Northwind.DataTransferObjects;
using Northwind.WebRole.Commands;
using Northwind.WebRole.Decorators;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Business;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;
using Unity;

namespace Northwind.WebRole.Services
{
    public class CustomerCrudService : CrudService<IBusinessUnitOfWork, Customer, CustomerDto>, ICustomerCrudService
    {
        public CustomerCrudService(IUnityContainer container) : base(container)
        {
        }

        public void Move()
        {
            try
            {
                MoveCustomerCommand command = new MoveCustomerCommand
                {
                    CustomerId = 1,
                    NewAddress = new Address
                    {
                        Description = "Address"
                    }
                };
                ICommandHandler<MoveCustomerCommand> handler =
                    Container.Resolve<ICommandHandler<MoveCustomerCommand>>(
                        HandlerDecoratorType.ValidationCommand.ToString());
                handler.Handle(command);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), HttpStatusCode.InternalServerError);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Select")]
        public override Stream SelectA(string page, string pageSize)
        {
            return base.SelectA(page, pageSize);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Select")]
        public override Stream SelectB(string page, string pageSize, string orderby)
        {
            return base.SelectB(page, pageSize, orderby);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Select")]
        public override Stream SelectC(string page, string pageSize, string orderby, string filter)
        {
            return base.SelectC(page, pageSize, orderby, filter);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Select")]
        public override Stream SelectD(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.SelectD(page, pageSize, orderby, filter, select);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Insert")]
        public override void Insert(CustomerDto dto)
        {
            base.Insert(dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Update")]
        public override void Update(string id, CustomerDto dto)
        {
            base.Update(id, dto);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Update")]
        public override void PartialUpdate(string id, string data)
        {
            base.PartialUpdate(id, data);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Delete")]
        public override void DeleteById(string id)
        {
            base.DeleteById(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "CustomerCrud.Select")]
        public override Stream SelectById(string id)
        {
            return base.SelectById(id);
        }

        public static void Configure(ServiceConfiguration config)
        {
            Configure<ICustomerCrudService>(config, "");
        }
    }
}