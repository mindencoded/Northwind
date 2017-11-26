using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SqlConnection connection) : base(connection)
        {
        }

        public UserRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public UserRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public UserRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(connection,
            transaction, ignoreNulls)
        {
        }

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}