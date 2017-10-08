using S3K.RealTimeOnline.DataAccess.UnitOfWorks;

namespace S3K.RealTimeOnline.DataAccess.Commands.MoveCustomer
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
