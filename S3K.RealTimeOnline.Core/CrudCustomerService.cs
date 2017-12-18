using System.IO;
using System.Net;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.Contracts;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.CommandHandlers;
using S3K.RealTimeOnline.GenericDataAccess.QueryHandlers;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.GenericDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public partial class MainService : ICrudCustomerService
    {
        public Stream SelectCustomer()
        {
            var query = new GenericSelectQuery
            {
                Page = 1,
                PageSize = 10
            };

            var handler = _container.Resolve<GenericSelectQueryHandler<IBusinessUnitOfWork, Customer>>();
            var data = handler.Handle(query);
            var response = ResponseDataToString(data);
            return JsonStream(response);
        }

        public void InsertCustomer(CustomerDto command)
        {
            var mapper = new Mapper<CustomerDto, Customer>();
            var customer = mapper.CreateMappedObject(command);
            var handler = _container.Resolve<GenericInsertCommandHandler<IBusinessUnitOfWork, Customer>>();
            handler.Handle(customer);
            var context = WebOperationContext.Current;
            if (context != null) context.OutgoingResponse.StatusCode = HttpStatusCode.Created;
        }

        public void UpdateCustomer(string id, CustomerDto command)
        {
            var customer = new Mapper<CustomerDto, Customer>().CreateMappedObject(command);
            var query = new GenericSelectByIdQuery<Customer>
            {
                Id = id
            };    
            var queryHandler = _container.Resolve<GenericSelectByIdQueryHandler<IBusinessUnitOfWork, Customer>>();
            var result = queryHandler.Handle(query);
            if (result != null)
            {
                var commandHandler = _container.Resolve<GenericUpdateCommandHandler<IBusinessUnitOfWork, Customer>>();
                commandHandler.Handle(customer);
                var context = WebOperationContext.Current;
                if (context != null) context.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteCustomerById(string id)
        {
            var handler = _container.Resolve<GenericDeleteByIdCommandHandler<IBusinessUnitOfWork, Customer>>();
            handler.Handle(id);
            var context = WebOperationContext.Current;
            if (context != null) context.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
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
            var handler = _container.Resolve<ICommandHandler<MoveCustomerCommand>>();
            handler.Handle(command);
        }
    }
}