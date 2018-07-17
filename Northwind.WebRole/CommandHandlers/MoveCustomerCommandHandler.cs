using System.Threading.Tasks;
using Northwind.WebRole.Tools;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.CommandHandlers
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