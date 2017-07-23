using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Security;

namespace S3K.RealTimeOnline.DataAccess.Commands.Security.MoveCustomer
{
    public class MoveCustomerCommandHandler : ICommandHandler<MoveCustomerCommand>
    {
        private readonly SecurityUnitOfWork _db;

        public MoveCustomerCommandHandler(SecurityUnitOfWork  db)
        {
            _db = db;
        }

        public virtual void Handle(MoveCustomerCommand command)
        {

        }
    }
}
