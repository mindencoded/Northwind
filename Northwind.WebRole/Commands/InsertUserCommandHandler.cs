using System;
using System.Threading.Tasks;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.UnitOfWork;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Commands
{
    public class InsertUserCommandHandler : ICommandHandler<InsertUserCommand>
    {
        private readonly ISecurityUnitOfWork _unitOfWork;

        public InsertUserCommandHandler(ISecurityUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Handle(InsertUserCommand command)
        {
            User user = new User
            {
                Username = command.Name,
                Active = true,
                Password = command.Password
            };

            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IRepository<User> repository = _unitOfWork.Repository<User>();
                repository.Insert(user);
            }
        }

        public Task HandleAsync(InsertUserCommand command)
        {
            throw new NotImplementedException();
        }
    }
}