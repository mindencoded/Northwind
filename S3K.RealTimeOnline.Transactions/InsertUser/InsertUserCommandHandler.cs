﻿using System;
using S3K.RealTimeOnline.GenericDataAccess.Commands;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.Transactions.InsertUser
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
                IRepository<User> repository = _unitOfWork.Repository<User>();
                repository.Insert(user);
                _unitOfWork.Commit();
            }
        }
    }
}