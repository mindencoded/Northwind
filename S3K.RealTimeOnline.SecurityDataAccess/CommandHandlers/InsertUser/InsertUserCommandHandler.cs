using System;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.CommandHandlers.InsertUser
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
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                Password = command.Password
            };

            using (_unitOfWork)
            {
                _unitOfWork.Open();
                IRepository<User> repository = _unitOfWork.Repository<User>();
                repository.Insert(user);
            }
        }
    }
}