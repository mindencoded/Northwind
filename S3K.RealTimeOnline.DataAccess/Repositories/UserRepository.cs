using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks;
using S3K.RealTimeOnline.Domain;

namespace S3K.RealTimeOnline.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public UserRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : base(sqlConnection,
            sqlTransaction)
        {
        }

        public UserRepository(ISecurityUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}