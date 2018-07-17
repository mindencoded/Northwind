using System.Data.SqlClient;
using S3K.RealTimeOnline.WebService.UnitOfWork;

namespace S3K.RealTimeOnline.WebService.Repositories
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