using System.Data.SqlClient;
using S3K.RealTimeOnline.DataAccess.UnitOfWorks.Security;
using S3K.RealTimeOnline.Domain.Entities.Security;

namespace S3K.RealTimeOnline.DataAccess.Repositories.Security
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SqlConnection sqlConnection) : base(sqlConnection)
        {
        }

        public UserRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : base(sqlConnection, sqlTransaction)
        {
        }

        public UserRepository(ISecurityUnitOfWork unitOfWork) : base(unitOfWork)
        {       
        }
    }
}