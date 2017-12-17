using System.Threading.Tasks;
using S3K.RealTimeOnline.BusinessDataAccess.UnitOfWork;
using S3K.RealTimeOnline.GenericDataAccess.Tools;

namespace S3K.RealTimeOnline.BusinessDataAccess.CommandHandlers.MoveCustomer
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

        public virtual Task HandleAsync(MoveCustomerCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}