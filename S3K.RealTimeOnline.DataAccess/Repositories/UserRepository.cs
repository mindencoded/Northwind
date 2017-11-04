using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SqlConnection connection) : base(connection)
        {
        }

        public UserRepository(SqlConnection connection, SqlTransaction transaction) : base(connection,
            transaction)
        {
        }

        public UserRepository(ISecurityUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}