﻿using System;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class CustomerCrudService : CrudService<IBusinessUnitOfWork, Customer, CustomerDto>, ICustomerCrudService
    {
        public CustomerCrudService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<ICustomerCrudService>(config, "");
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
        public override Stream Select(string page, string pageSize, string orderby, string filter, string select)
        {
            return base.Select(page, pageSize, orderby, filter, select);
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
    }
}