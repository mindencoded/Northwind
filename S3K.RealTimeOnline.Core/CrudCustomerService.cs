using System.IO;
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