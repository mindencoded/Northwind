using System.Data.SqlClient;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories;

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
    }
}