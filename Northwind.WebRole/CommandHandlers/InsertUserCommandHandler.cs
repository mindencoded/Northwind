using System;
using System.Threading.Tasks;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Repositories;
using Northwind.WebRole.Tools;
using Northwind.WebRole.UnitOfWork;

namespace Northwind.WebRole.CommandHandlers
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