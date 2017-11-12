using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Transactions.CustomerCommandHandlers
{
    public class CustomerInsertCommandHandler : ICommandHandler<Customer>
    {
        private readonly IBusinessUnitOfWork _unitOfWork;

        public CustomerInsertCommandHandler(IBusinessUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle(Customer command)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.Repository<Customer>().Insert(command);
            }
        }
    }
}