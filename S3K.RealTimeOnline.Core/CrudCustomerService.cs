using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public partial class MainService : ICrudCustomerService
    {
        public Stream SelectCustomer(string page, string pageSize)
        {
            if (string.IsNullOrEmpty(page))
            {
                throw new ArgumentException("The 'page' parameter cannot be null or empty");
            }

            if (string.IsNullOrEmpty(pageSize))
            {
                throw new ArgumentException("The 'pageSize' parameter cannot be null or empty");
            }

            var query = new GenericSelectQuery
            {
                Page = Convert.ToInt32(page) == 0 ? 1 : Convert.ToInt32(page),
                PageSize = Convert.ToInt32(pageSize) == 0 ? 100 : Convert.ToInt32(pageSize)
            };

            var handler = Container.Resolve<GenericSelectQueryHandler<IBusinessUnitOfWork>>();
            var data = handler.Handle<Customer>(query);
            var response = ResponseDataToString(data);
            return JsonStream(response);
        }

        public Stream SelectCustomerById(string id)
        {
            var query = new GenericSelectByIdQuery
            {
                Id = id
            };
            var handler = Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
            var entity = handler.Handle(query) as Customer;
            var response = ResponseDataToString(entity);
            return JsonStream(response);
        }

        public void InsertCustomer(CustomerDto command)
        {
            var mapper = new Mapper<CustomerDto, Customer>();
            var customer = mapper.CreateMappedObject(command);
            var commandHandler = ResolveGenericCommandHandler(HandlerDecoratorType.ValidationCommand,
                UnitOfWorkType.Business, GenericCommandType.Insert);
            commandHandler.Handle<Customer>(customer);
            if (WebOperationContext.Current != null) WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Created;
        }

        public void UpdateCustomer(string id, CustomerDto command)
        {
            if (string.IsNullOrEmpty(id) || Convert.ToInt32(id) == 0)
            {
                throw new ArgumentException("The 'id' parameter cannot be null, zero or empty");
            }

            var customer = new Mapper<CustomerDto, Customer>().CreateMappedObject(command);
            var query = new GenericSelectByIdQuery
            {
                Id = id
            };
            var queryHandler = Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
            var result = queryHandler.Handle(query);
            if (result != null)
            {
                if (customer.Id != int.Parse(id)) customer.Id = Convert.ToInt32(id);
                var commandHandler = ResolveGenericCommandHandler(HandlerDecoratorType.ValidationCommand,
                    UnitOfWorkType.Business, GenericCommandType.Update);
                commandHandler.Handle<Customer>(customer);
                if (WebOperationContext.Current != null) WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }
        }

        public void PartialUpdateCustomer(string id, string json)
        {
            if (string.IsNullOrEmpty(id) || Convert.ToInt32(id) == 0)
            {
                throw new ArgumentException("The 'id' parameter cannot be null, zero or empty");
            }

            IDictionary<string, object> obj = DeserializeToDictionary<Customer>(json);
            var query = new GenericSelectByIdQuery
            {
                Id = id
            };

            var queryHandler = Container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
            var result = queryHandler.Handle(query);
            if (result != null)
            {
                if (!obj.ContainsKey("Id") || obj["Id"].ToString() != id)
                {
                    obj["Id"] = Convert.ToInt32(id);
                }
                var commandHandler = ResolveGenericCommandHandler(HandlerDecoratorType.ValidationCommand,
                    UnitOfWorkType.Business, GenericCommandType.Update);
                commandHandler.Handle<Customer>(obj);
                if (WebOperationContext.Current != null) WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteCustomerById(string id)
        {
            var commandHandler = ResolveGenericCommandHandler(HandlerDecoratorType.TransactionCommand,
                UnitOfWorkType.Business, GenericCommandType.DeleteById);
            commandHandler.Handle<Customer>(id);
            if (WebOperationContext.Current != null) WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
        }

        public void MoveCustomer()
        {
            var command = new MoveCustomerCommand
            {
                CustomerId = 1,
                NewAddress = new Address
                {
                    Description = "Address"
                }
            };
            var handler =
                Container.Resolve<ICommandHandler<MoveCustomerCommand>>(
                    HandlerDecoratorType.ValidationCommand.ToString());
            handler.Handle(command);
        }
    }
}