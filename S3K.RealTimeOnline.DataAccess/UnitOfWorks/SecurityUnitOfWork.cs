﻿using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.Repositories;

namespace S3K.RealTimeOnline.DataAccess.UnitOfWorks
{
    public class SecurityUnitOfWork : UnitOfWork, ISecurityUnitOfWork
    {
        public SecurityUnitOfWork(SqlConnection sqlConnection, bool isTransactional = true) : base(sqlConnection, isTransactional)
        {
        }


        public IUserRepository UserRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(UserRepository)))
                {
                    Repositories.Add(typeof(UserRepository), new UserRepository(SqlConnection, SqlTransaction));
                }

                return (IUserRepository) Repositories[typeof(UserRepository)];
            }
        }
    }
}