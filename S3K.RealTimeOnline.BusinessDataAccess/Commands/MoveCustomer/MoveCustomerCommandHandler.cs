using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWorks;
using S3K.RealTimeOnline.GenericDataAccess.Commands;

namespace S3K.RealTimeOnline.BusinessDataAccess.Commands.MoveCustomer
{
    public class MoveCustomerCommandHandler : ICommandHandler<MoveCustomerCommand>
    {
        private readonly IBusinessUnitOfWork _db;

        public MoveCustomerCommandHandler(IBusinessUnitOfWork db)
        {
            _db = db;
        }

        public virtual void Handle(MoveCustomerCommand command)
        {
        }
    }
}