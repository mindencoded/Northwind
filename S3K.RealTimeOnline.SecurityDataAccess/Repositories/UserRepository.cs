using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.SecurityDomain;
using ISecurityUnitOfWork = S3K.RealTimeOnline.SecurityDataAccess.UnitOfWork.ISecurityUnitOfWork;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
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