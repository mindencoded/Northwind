using S3K.RealTimeOnline.BusinessDataAccess.Repositories;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Transactions.CustomerCommandHandlers
{
    public class CustomerUpdateCommandHandler : ICommandHandler<Customer>
    {
        private readonly IBusinessUnitOfWork _unitOfWork;

        public CustomerUpdateCommandHandler(IBusinessUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual void Handle(Customer command)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                ICustomerRepository repository = new CustomerRepository(_unitOfWork, false);
                repository.Update(command);
            }
        }
    }
}
