using System.Data.SqlClient;
using S3K.RealTimeOnline.GenericDataAccess.Repositories;
using S3K.RealTimeOnline.GenericDataAccess.UnitOfWork;
using S3K.RealTimeOnline.SecurityDataAccess.Repositories.Contracts;
using S3K.RealTimeOnline.SecurityDomain;

namespace S3K.RealTimeOnline.SecurityDataAccess.Repositories
{
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(SqlConnection connection) : base(connection)
        {
        }

        public UserTypeRepository(SqlConnection connection, bool ignoreNulls) : base(connection, ignoreNulls)
        {
        }

        public UserTypeRepository(SqlConnection connection, SqlTransaction transaction) : base(connection, transaction)
        {
        }

        public UserTypeRepository(SqlConnection connection, SqlTransaction transaction, bool ignoreNulls) : base(
            connection, transaction, ignoreNulls)
        {
        }

        public UserTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserTypeRepository(IUnitOfWork unitOfWork, bool ignoreNulls) : base(unitOfWork, ignoreNulls)
        {
        }
    }
}