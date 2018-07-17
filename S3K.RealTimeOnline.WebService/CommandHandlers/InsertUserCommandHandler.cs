using System;
using System.Threading.Tasks;
using S3K.RealTimeOnline.WebService.Repositories;
using S3K.RealTimeOnline.WebService.Tools;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.CommandHandlers
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