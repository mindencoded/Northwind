using System;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public partial class BusinessService : ICustomerMaintenanceService
    {
        public Stream SelectCustomer(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, Customer>(page, pageSize);
        }

        public Stream SelectCustomerById(string id)
        {
            return SelectById<IBusinessUnitOfWork, Customer>(id);
        }

        public void InsertCustomer(CustomerDto command)
        {
            Insert<IBusinessUnitOfWork, Customer, CustomerDto>(command);
        }

        public void UpdateCustomer(string id, CustomerDto command)
        {
            Update<IBusinessUnitOfWork, Customer, CustomerDto>(id, command);
        }

        public void PartialUpdateCustomer(string id, string json)
        {
            PartialUpdate<IBusinessUnitOfWork, Customer>(id, json);
        }

        public void DeleteCustomerById(string id)
        {
            DeleteById<IBusinessUnitOfWork, Customer>(id);
        }

        public void MoveCustomer()
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
    }
}