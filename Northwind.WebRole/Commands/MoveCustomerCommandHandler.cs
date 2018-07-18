using System.Threading.Tasks;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Commands
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