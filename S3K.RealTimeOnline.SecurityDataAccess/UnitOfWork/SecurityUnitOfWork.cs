using System.Data.SqlClient;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;

namespace S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork
{
    public class SecurityUnitOfWork : GenericDataAccess.UnitOfWork.UnitOfWork, ISecurityUnitOfWork
    {
        public SecurityUnitOfWork(SqlConnection connection) : base(connection)
        {
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(UserRepository)))
                    Repositories.Add(typeof(UserRepository), new UserRepository(Connection, Transaction));
                return (IUserRepository) Repositories[typeof(UserRepository)];
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(RoleRepository)))
                    Repositories.Add(typeof(RoleRepository), new RoleRepository(Connection, Transaction));
                return (IRoleRepository) Repositories[typeof(RoleRepository)];
            }
        }

        public IRoleDetailRepository RoleDetailRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(RoleDetailRepository)))
                    Repositories.Add(typeof(RoleDetailRepository), new RoleDetailRepository(Connection, Transaction));
                return (IRoleDetailRepository) Repositories[typeof(RoleDetailRepository)];
            }
        }

        public IUserTypeRepository UserTypeRepository
        {
            get
            {
                if (!Repositories.ContainsKey(typeof(UserTypeRepository)))
                    Repositories.Add(typeof(UserTypeRepository), new UserTypeRepository(Connection, Transaction));
                return (IUserTypeRepository) Repositories[typeof(UserTypeRepository)];
            }
        }
    }
}