using System;
using System.IO;
using System.Net;
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
    public partial class MaintenanceService : ICustomerMaintenanceService
    {
        public Stream SelectCustomer(string page, string pageSize)
        {
            return Select<IBusinessUnitOfWork, Customer>(page, pageSize);
        }

        public Stream SelectCustomerById(string id)
        {
            return SelectById<IBusinessUnitOfWork, Customer>(id);
        }

        public void InsertCustomer(CustomerDto dto)
        {
            Insert<IBusinessUnitOfWork, Customer, CustomerDto>(dto);
        }

        public void UpdateCustomer(string id, CustomerDto dto)
        {
            Update<IBusinessUnitOfWork, Customer, CustomerDto>(id, dto);
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