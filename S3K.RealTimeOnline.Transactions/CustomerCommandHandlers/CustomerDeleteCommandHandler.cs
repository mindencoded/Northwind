using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.BusinessDomain;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.Transactions.CustomerCommandHandlers
{
    public class CustomerDeleteCommandHandler : ICommandHandler<int>
    {
        private readonly IBusinessUnitOfWork _unitOfWork;

        public CustomerDeleteCommandHandler(IBusinessUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle(int command)
        {
            using (_unitOfWork)
            {
                _unitOfWork.Open();
                _unitOfWork.Repository<Customer>().Delete(command);
            }
        }
    }
}
